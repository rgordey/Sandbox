// Application/Features/Orders/Queries/GetOrdersQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrdersQuery : IQuery<List<OrderDto>>
    {
        public Guid? CustomerId { get; set; } // Optional filter by customer
    }

    internal sealed class GetOrdersQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Orders.AsQueryable();
            if (request.CustomerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == request.CustomerId.Value);
            }
            return await query
                .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}