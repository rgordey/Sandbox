using Application.Core;
using Application.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Tests
{
    public class DtoQueryTests : IAsyncLifetime
    {
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DtoQueryTests()
        {
            Console.WriteLine("DtoQueryTests constructor started.");
            try
            {
                var services = new ServiceCollection()
                    .AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("Server=.;Database=EfCoreBenchmark;Trusted_Connection=True;TrustServerCertificate=True;"))
                    .AddAutoMapper(typeof(MappingProfile))
                    .BuildServiceProvider();

                _scope = services.CreateScope();
                _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
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

            Assert.NotEmpty(dtos); // Ensure data is returned
            var customer1 = dtos.First();
            Assert.Equal("First89 Last89", customer1.FullName); // Based on your seed data
            Assert.Equal("customer89@example.com", customer1.Email);
            Assert.Equal(3, customer1.Orders.Count); // 3 orders per customer in your seed
            Assert.Equal(2, customer1.Orders[0].OrderDetails.Count); // 2 details per order
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
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

            Assert.NotEmpty(dtos);
            var customer1 = dtos.First();
            Assert.Equal("First89 Last89", customer1.FullName);
            Assert.Equal("customer89@example.com", customer1.Email);
            Assert.Equal(3, customer1.Orders.Count);
            Assert.Equal(2, customer1.Orders[0].OrderDetails.Count);
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
        }

        [Fact]
        public async Task AutoMapperProjectToAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running AutoMapperProjectToAsync test.");
            var dtos = await _context.Customers
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .Take(10) // Limit to first 10 for testing, adjust as needed
                .ToListAsync();

            Assert.NotEmpty(dtos);
            var customer1 = dtos.First();
            Assert.Equal("First89 Last89", customer1.FullName);
            Assert.Equal("customer89@example.com", customer1.Email);
            Assert.Equal(3, customer1.Orders.Count);
            Assert.Equal(2, customer1.Orders[0].OrderDetails.Count);
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
        }
    }
}