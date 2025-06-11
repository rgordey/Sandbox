using Application;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BenchmarkEFCore.Tests
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
                        options.UseSqlite("DataSource=:memory:"))
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
                Console.WriteLine("Ensuring database is deleted.");
                await _context.Database.EnsureDeletedAsync();
                Console.WriteLine("Ensuring database is created.");
                await _context.Database.EnsureCreatedAsync();
                // Verify schema creation
                var tables = await _context.Database.SqlQuery<string>($"SELECT name FROM sqlite_master WHERE type='table';").ToListAsync();
                Console.WriteLine($"Created tables: {string.Join(", ", tables)}");
                Console.WriteLine("Seeding database.");
                await TestData.SeedDatabaseAsync(_context); // Use async method
                Console.WriteLine("Database seeded successfully.");
                var customerCount = await _context.Customers.CountAsync();
                Console.WriteLine($"Verified {customerCount} customers in the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding failed: {ex.Message}");
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
            await InitializeAsync(); // Force initialization
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
                .ToListAsync();

            Assert.Equal(3, dtos.Count);
            var customer1 = dtos.First(c => c.Id == 1);
            Assert.Equal("First1 Last1", customer1.FullName);
            Assert.Equal("customer1@example.com", customer1.Email);
            Assert.Equal(2, customer1.Orders.Count);
            Assert.Equal(1, customer1.Orders[0].OrderDetails.Count);
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
        }

        [Fact]
        public async Task EfCoreLinqQueryAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running EfCoreLinqQueryAsync test.");
            await InitializeAsync(); // Force initialization
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
                              }).ToListAsync();

            Assert.Equal(3, dtos.Count);
            var customer1 = dtos.First(c => c.Id == 1);
            Assert.Equal("First1 Last1", customer1.FullName);
            Assert.Equal("customer1@example.com", customer1.Email);
            Assert.Equal(2, customer1.Orders.Count);
            Assert.Equal(1, customer1.Orders[0].OrderDetails.Count);
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
        }

        [Fact]
        public async Task AutoMapperProjectToAsync_ReturnsAccurateData()
        {
            Console.WriteLine("Running AutoMapperProjectToAsync test.");
            await InitializeAsync(); // Force initialization
            var dtos = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderDetails)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            Assert.Equal(3, dtos.Count);
            var customer1 = dtos.First(c => c.Id == 1);
            Assert.Equal("First1 Last1", customer1.FullName);
            Assert.Equal("customer1@example.com", customer1.Email);
            Assert.Equal(2, customer1.Orders.Count);
            Assert.Equal(1, customer1.Orders[0].OrderDetails.Count);
            Assert.Equal("Product1", customer1.Orders[0].OrderDetails[0].ProductName);
        }
    }
}