using Application.Core;
using Application.Core.Common.Behaviors;
using Application.Core.Common.Interfaces;
using Application.Core.Features.Customers.Commands;
using Application.Core.Features.Customers.Queries;
using Application.Core.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Tests
{
    public class DtoQueryTests : IAsyncLifetime
    {
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DtoQueryTests()
        {
            Console.WriteLine("DtoQueryTests constructor started.");
            try
            {
                var services = new ServiceCollection()
                    .AddDbContext<IAppDbContext, AppDbContext>(options =>
                        options.UseSqlServer("Server=.;Database=EfCoreBenchmark;Trusted_Connection=True;TrustServerCertificate=True;"))
                    .AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)
                    .AddAutoMapper(action => action.AddProfile<MappingProfile>())
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(typeof(GetCustomersQuery).Assembly);
                        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                    })
                    .BuildServiceProvider();

                _scope = services.CreateScope();
                _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
                _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
                Console.WriteLine("DtoQueryTests constructor completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DtoQueryTests constructor failed: {ex.Message}");
                throw;
            }
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine("InitializeAsync started.");
            try
            {
                // No seeding needed; verify connection
                Console.WriteLine("Verifying database connection.");
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Database connection successful: {canConnect}");
                if (!canConnect)
                {
                    throw new Exception("Failed to connect to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization failed: {ex.Message}");
                throw;
            }
            Console.WriteLine("InitializeAsync completed.");
        }

        public async Task DisposeAsync()
        {
            Console.WriteLine("DisposeAsync started.");
            _scope.Dispose();
            Console.WriteLine("DisposeAsync completed.");
        }

        [Fact]
        public async Task EfCoreNormalQueryAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running EfCoreNormalQueryAsync test.");
            var dtos = await _context.Customers
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
                .Take(10) // Limit to first 10 for testing, adjust as needed
                .ToListAsync();

            
            using (new AssertionScope())
            {
                dtos.Should().NotBeEmpty(); // Ensure data is returned
                var customer1 = dtos.First();

                customer1.FullName.Should().Be("First824 Last824"); // Based on your seed data            
                customer1.Email.Should().Be("customer824@example.com"); // Based on your seed data            
                customer1.Orders.Should().HaveCount(3); // 3 orders per customer in your seed
                customer1.Orders[0].OrderDetails.Should().HaveCount(2); // 2 details per order  
                customer1.Orders[0].OrderDetails[0].ProductName.Should().Be("Product1"); // Based on your seed data
            }
        }

        [Fact]
        public async Task EfCoreLinqQueryAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running EfCoreLinqQueryAsync test.");
            var dtos = await (from c in _context.Customers
                              .Include(c => c.Orders)
                                .ThenInclude(o => o.OrderDetails)
                              select new CustomerDto
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
                              .Take(10) // Limit to first 10 for testing, adjust as needed
                              .ToListAsync();

            using (new AssertionScope())
            {
                dtos.Should().NotBeEmpty(); // Ensure data is returned
                var customer1 = dtos.First();

                customer1.FullName.Should().Be("First824 Last824"); // Based on your seed data            
                customer1.Email.Should().Be("customer824@example.com"); // Based on your seed data            
                customer1.Orders.Should().HaveCount(3); // 3 orders per customer in your seed
                customer1.Orders[0].OrderDetails.Should().HaveCount(2); // 2 details per order  
                customer1.Orders[0].OrderDetails[0].ProductName.Should().Be("Product1"); // Based on your seed data
            }
        }

        [Fact]
        public async Task AutoMapperProjectToAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running AutoMapperProjectToAsync test.");
            var dtos = await _context.Customers
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .Take(10) // Limit to first 10 for testing, adjust as needed
                .ToListAsync();

            using (new AssertionScope())
            {
                dtos.Should().NotBeEmpty(); // Ensure data is returned
                var customer1 = dtos.First();

                customer1.FullName.Should().Be("First824 Last824"); // Based on your seed data            
                customer1.Email.Should().Be("customer824@example.com"); // Based on your seed data            
                customer1.Orders.Should().HaveCount(3); // 3 orders per customer in your seed
                customer1.Orders[0].OrderDetails.Should().HaveCount(2); // 2 details per order  
                customer1.Orders[0].OrderDetails[0].ProductName.Should().Be("Product1"); // Based on your seed data
            }            
        }

        [Fact]
        public async Task UpdateCustomerCommand_InvalidCustomerId_ThrowsValidationException()
        {
            var act = () => _mediator.Send(new UpdateCustomerCommand() { Customer = new CustomerDto() { Id = Guid.Empty } });

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("User not found");
        }
    }
}