using Application;
using AutoMapper;
using Domain;
using Xunit;

namespace BenchmarkEFCore.Tests
{
    public class MapperTests
    {
        private readonly IMapper _mapper;

        public MapperTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
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
            var customer = new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Orders = new List<Order>
                {
                    new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.UtcNow, TotalAmount = 100m }
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
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100m,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { Id = 1, OrderId = 1, ProductName = "Product1", Quantity = 1, UnitPrice = 50m }
                }
            };

            var dto = _mapper.Map<OrderDto>(order);

            Assert.Equal(order.Id, dto.Id);
            Assert.Equal(order.OrderDate, dto.OrderDate);
            Assert.Equal(order.TotalAmount, dto.TotalAmount);
            Assert.Single(dto.OrderDetails);
            Assert.Equal(order.OrderDetails[0].Id, dto.OrderDetails[0].Id);
        }

        [Fact]
        public void Map_OrderDetailToDto_MapsCorrectly()
        {
            var orderDetail = new OrderDetail
            {
                Id = 1,
                OrderId = 1,
                ProductName = "Product1",
                Quantity = 1,
                UnitPrice = 50m
            };

            var dto = _mapper.Map<OrderDetailDto>(orderDetail);

            Assert.Equal(orderDetail.Id, dto.Id);
            Assert.Equal(orderDetail.ProductName, dto.ProductName);
            Assert.Equal(orderDetail.Quantity, dto.Quantity);
            Assert.Equal(orderDetail.UnitPrice, dto.UnitPrice);
        }
    }
}
