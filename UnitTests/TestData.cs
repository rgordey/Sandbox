using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkEFCore.Tests
{
    public class TestData
    {
        public static async Task SeedDatabaseAsync(AppDbContext context)
        {
            var customers = Enumerable.Range(1, 3).Select(i => new Customer
            {
                Id = i,
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Email = $"customer{i}@example.com"
            }).ToList();

            var orders = new List<Order>();
            int orderId = 1;
            foreach (var customer in customers)
            {
                for (int i = 0; i < 2; i++)
                {
                    orders.Add(new Order
                    {
                        Id = orderId++,
                        CustomerId = customer.Id,
                        OrderDate = DateTime.UtcNow.AddDays(-i),
                        TotalAmount = 100m * (i + 1)
                    });
                }
            }

            var orderDetails = new List<OrderDetail>();
            int detailId = 1;
            foreach (var order in orders)
            {
                for (int i = 0; i < 1; i++)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        Id = detailId++,
                        OrderId = order.Id,
                        ProductName = $"Product{i + 1}",
                        Quantity = i + 1,
                        UnitPrice = 50m * (i + 1)
                    });
                }
            }

            await context.Customers.AddRangeAsync(customers);
            await context.Orders.AddRangeAsync(orders);
            await context.OrderDetails.AddRangeAsync(orderDetails);
            await context.SaveChangesAsync();
        }
    }
}