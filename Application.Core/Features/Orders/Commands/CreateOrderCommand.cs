// Updated Application/Features/Orders/Commands/CreateOrderCommand.cs
using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands
{
    public sealed class CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class CreateOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
        {

            var order = mapper.Map<SalesOrder>(request);
            order.Id = Guid.NewGuid();
            order.Status = SalesOrderStatus.Placed;
            decimal baseTotal = 0m;

            foreach (var detail in order.OrderDetails)
            {
                var product = await context.Products
                    .Include(p => p.ProductVendors)
                    .FirstOrDefaultAsync(p => p.Id == detail.ProductId, ct);
                if (product == null)
                {
                    throw new NotFoundException($"Product {detail.ProductId} not found.");
                }

                // Inventory availability: Check total stock across all vendors
                var totalStock = product.ProductVendors.Sum(pv => pv.StockQuantity);
                if (totalStock < detail.Quantity)
                {
                    throw new ValidationException($"Insufficient stock for product {product.Name}. Available: {totalStock}, Requested: {detail.Quantity}.");
                }

                // Price: Use retail price (with standard markup on min cost)
                var minCost = product.ProductVendors.Any() ? product.ProductVendors.Min(pv => pv.VendorPrice) : product.BasePrice;
                var retailPrice = minCost * 2m;  // Standard 100% markup for retail price
                detail.UnitPrice = retailPrice;

                // Select vendor for stock reduction (e.g., cheapest with enough stock)
                var availableVendors = product.ProductVendors.Where(pv => pv.StockQuantity >= detail.Quantity).OrderBy(pv => pv.VendorPrice).ToList();
                var selectedVendor = availableVendors.First();  // Cheapest with sufficient stock

                detail.Id = Guid.NewGuid();
                detail.OrderId = order.Id;
                detail.Product = null;

                // Reduce stock from selected vendor
                selectedVendor.StockQuantity -= detail.Quantity;

                baseTotal += detail.Quantity * detail.UnitPrice;
            }

            // Load customer and apply discount
            var customer = await context.Customers.FindAsync(new object?[] { request.CustomerId }, cancellationToken: ct);
            if (customer == null)
            {
                throw new NotFoundException($"Customer {request.CustomerId} not found.");
            }

            decimal discountRate = customer switch
            {
                ResidentialCustomer res => res.IsSeniorDiscountEligible ? 0.10m : 0m,
                CorporateCustomer corp => corp.EmployeeCount > 100 ? 0.05m : 0m,
                GovernmentCustomer gov => gov.IsFederal ? 0.10m : 0m,
                _ => 0m
            };

            order.TotalAmount = baseTotal * (1 - discountRate);

            context.Orders.Add(order);
            await context.SaveChangesAsync(ct);
            return order.Id;
        }
    }
}