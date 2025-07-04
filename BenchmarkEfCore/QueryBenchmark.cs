using Application.Core;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BenchmarkDotNet.Attributes;
using Infrastructure;
using Mapster;
using Mapster.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Domain;

namespace Presentation.Benchmark
{
    [MemoryDiagnoser]
    public class QueryBenchmark
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _connectionString = "Server=.;Database=EfCoreBenchmark;Trusted_Connection=True;TrustServerCertificate=True;";

        public QueryBenchmark()
        {
            var services = new ServiceCollection();

            // Configure EF Core with SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_connectionString, opt =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }), ServiceLifetime.Transient);

            services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);

            // Configure AutoMapper
            services.AddAutoMapper(action =>
            {
                action.AddProfile<MappingProfile>();
            });

            // Configure Mapster
            TypeAdapterConfig.GlobalSettings
                .ForType<Customer, CustomerDto>()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName);

            TypeAdapterConfig.GlobalSettings
                .ForType<CustomerDto, Customer>()
                .Map(dest => dest.FirstName, src => src.FullName.Split(new[] { ' ' }, 2)[0])
                .Map(dest => dest.LastName, src => src.FullName.Split(new[] { ' ' }, 2)[1]);

            TypeAdapterConfig.GlobalSettings
                .ForType<Order, OrderDto>();

            TypeAdapterConfig.GlobalSettings
                .ForType<OrderDto, Order>();

            TypeAdapterConfig.GlobalSettings
                .ForType<OrderDetail, OrderDetailDto>();

            TypeAdapterConfig.GlobalSettings
                .ForType<OrderDetailDto, OrderDetail>();

            TypeAdapterConfig.GlobalSettings.Compile();

            _serviceProvider = services.BuildServiceProvider(validateScopes: true);

            // Validate and pre-warm AutoMapper configuration
            using var scope = _serviceProvider.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.CompileMappings();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Ensure database is created (no seeding for remote DB)
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            // Warm up database connection and query cache
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderDetails)
                .Take(1)
                .ToList();
        }

        [Benchmark(Baseline = true)]
        public async Task<List<CustomerDto>> EfCoreNormalQueryAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderDetails)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FirstName + " " + c.LastName,
                    Email = c.Email,
                    Orders = c.Orders.Select(o => new OrderDto
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                        {
                            Id = od.Id,
                            ProductName = od.ProductName,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
        }

        [Benchmark]
        public async Task<List<CustomerDto>> EfCoreLinqQueryAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await (from c in context.Customers
                          join o in context.Orders on c.Id equals o.CustomerId into orders
                          from o in orders.DefaultIfEmpty()
                          join od in context.OrderDetails on o.Id equals od.OrderId into orderDetails
                          from od in orderDetails.DefaultIfEmpty()
                          group new { o, od } by new { c.Id, c.FirstName, c.LastName, c.Email } into g
                          select new CustomerDto
                          {
                              Id = g.Key.Id,
                              FullName = g.Key.FirstName + " " + g.Key.LastName,
                              Email = g.Key.Email,
                              Orders = g.Where(x => x.o != null).GroupBy(x => x.o.Id).Select(og => new OrderDto
                              {
                                  Id = og.Key,
                                  OrderDate = og.First().o.OrderDate,
                                  TotalAmount = og.First().o.TotalAmount,
                                  OrderDetails = og.Where(x => x.od != null).Select(x => new OrderDetailDto
                                  {
                                      Id = x.od.Id,
                                      ProductName = x.od.ProductName,
                                      Quantity = x.od.Quantity,
                                      UnitPrice = x.od.UnitPrice
                                  }).ToList()
                              }).ToList()
                          })
                          .ToListAsync();
        }

        [Benchmark]
        public async Task<List<CustomerDto>> AutoMapperProjectToAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            return await context.Customers
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [Benchmark]
        public async Task<List<CustomerDto>> MapsterProjectToAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await context.Customers
                .ProjectToType<CustomerDto>()
                .ToListAsync();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            (_serviceProvider as IDisposable)?.Dispose();
        }
    }
}