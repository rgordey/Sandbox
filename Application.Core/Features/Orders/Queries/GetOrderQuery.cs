// Application/Features/Orders/Queries/GetOrderQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrderQuery : IQuery<OrderDto?>
    {
        public Guid OrderId { get; set; }
    }

    internal sealed class GetOrderQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrderQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            return await context.Orders
                .Where(x => x.Id == query.OrderId)
                .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}