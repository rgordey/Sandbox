using Application;
using Application.Mappings;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging.Abstractions;

namespace ApplicationTests
{
    public class MapperTests
    {
        private readonly IMapper _mapper;

        public MapperTests()
        {
            var loggerFactory = NullLoggerFactory.Instance;
            var config = new MapperConfiguration(cfg => 
            {                
                cfg.AddMaps(typeof(MappingProfile).Assembly);
            }, loggerFactory);
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MappingConfiguration_IsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CustomerToDto_MapsCorrectly()
        {
            var customerId = Guid.NewGuid();
            
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Orders = new List<SalesOrder>
                {
                    new SalesOrder { Id = Guid.NewGuid(), CustomerId = customerId, OrderDate = DateTime.UtcNow, TotalAmount = 100m }
                }
            };

            var dto = _mapper.Map<CustomerDto>(customer);

            Assert.Equal(customer.Id, dto.Id);
            Assert.Equal("John Doe", dto.FullName);
            Assert.Equal(customer.Email, dto.Email);
            Assert.Single(dto.Orders);
            Assert.Equal(customer.Orders[0].Id, dto.Orders[0].Id);
        }

        [Fact]
        public void Map_OrderToDto_MapsCorrectly()
        {
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var order = new SalesOrder
            {
                Id = orderId,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100m,
                OrderDetails = new List<SalesOrderDetail>
                {
                    new SalesOrderDetail { Id = Guid.NewGuid(), OrderId = orderId, Quantity = 1, UnitPrice = 50m }
                }
            };

            var dto = _mapper.Map<SalesOrderDto>(order);

            Assert.Equal(order.Id, dto.Id);
            Assert.Equal(order.OrderDate, dto.OrderDate);
            Assert.Equal(order.TotalAmount, dto.TotalAmount);
            Assert.Single(dto.OrderDetails);
            Assert.Equal(order.OrderDetails[0].Id, dto.OrderDetails[0].Id);
        }

        [Fact]
        public void Map_OrderDetailToDto_MapsCorrectly()
        {
            var orderDetail = new SalesOrderDetail
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 50m
            };

            var dto = _mapper.Map<SalesOrderDetailDto>(orderDetail);

            Assert.Equal(orderDetail.Id, dto.Id);            
            Assert.Equal(orderDetail.Quantity, dto.Quantity);
            Assert.Equal(orderDetail.UnitPrice, dto.UnitPrice);
        }
    }
}
