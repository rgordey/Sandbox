// Application/Features/PurchaseOrders/Queries/GetPurchaseOrderQuery.cs (new)
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrders.Queries
{
    public sealed class GetPurchaseOrderQuery : IQuery<PurchaseOrderDto?>
    {
        public Guid PurchaseOrderId { get; set; }
    }

    internal sealed class GetPurchaseOrderQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetPurchaseOrderQuery, PurchaseOrderDto?>
    {
        public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            return await context.PurchaseOrders
                .Where(x => x.Id == request.PurchaseOrderId)
                .ProjectTo<PurchaseOrderDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}