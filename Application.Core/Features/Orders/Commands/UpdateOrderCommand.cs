// Application/Features/Orders/Commands/UpdateOrderCommand.cs
using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Application.Features.Orders.Commands
{
    public sealed class UpdateOrderCommand : ICommand<Unit>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int SequentialNumber { get; set; }
        public string? OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public SalesOrderStatus Status { get; set; }  // New: Status for update
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class UpdateOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken ct)
        {
            var order = await context.Orders
                .AsTracking()
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order == null)
            {
                throw new NotFoundException($"Order {request.OrderId} not found.");
            }

            // Update basic properties
            order.OrderDate = request.OrderDate;
            order.Status = request.Status;  // Update status

            decimal baseTotal = 0m;

            // Update details (simple sync; for production, use a diffing library like EFPlus)
            order.OrderDetails.Clear();
            foreach (var detailDto in request.OrderDetails)
            {
                var product = await context.Products
                    .Include(p => p.ProductVendors)
                    .FirstOrDefaultAsync(p => p.Id == detailDto.ProductId, ct);

                if (product == null)
                {
                    throw new NotFoundException($"Product {detailDto.ProductId} not found.");
                }

                // Recalculate unit price with min vendor
                var availableVendors = product.ProductVendors.Where(pv => pv.StockQuantity >= detailDto.Quantity).ToList();
                if (availableVendors.Count == 0)
                {
                    throw new ValidationException($"Insufficient stock for product {product.Name}.");
                }
                var selectedVendor = availableVendors.OrderBy(pv => pv.VendorPrice).First();

                var detail = mapper.Map<SalesOrderDetail>(detailDto);
                detail.UnitPrice = selectedVendor.VendorPrice;
                detail.OrderId = order.Id;
                detail.Product = null;  // Avoid navigation

                // Reduce stock (assuming update allows stock adjustment; revert on delete if needed)
                selectedVendor.StockQuantity -= detail.Quantity;

                order.OrderDetails.Add(detail);

                baseTotal += detail.Quantity * detail.UnitPrice;
            }

            // Reload customer for discount (assume no customer change on edit)
            var customer = await context.Customers.FindAsync(new object?[] { order.CustomerId }, cancellationToken: ct);

            decimal discountRate = customer switch
            {
                ResidentialCustomer res => res.IsSeniorDiscountEligible ? 0.10m : 0m,
                CorporateCustomer corp => corp.EmployeeCount > 100 ? 0.05m : 0m,
                GovernmentCustomer gov => gov.IsFederal ? 0.10m : 0m,
                _ => 0m
            };

            order.TotalAmount = baseTotal * (1 - discountRate);

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}