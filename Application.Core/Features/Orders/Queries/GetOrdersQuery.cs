// Application/Features/Orders/Queries/GetOrdersQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrdersQuery : IQuery<List<SalesOrderDto>>
    {
        public Guid? CustomerId { get; set; } // Optional filter by customer
    }

    internal sealed class GetOrdersQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrdersQuery, List<SalesOrderDto>>
    {
        public async Task<List<SalesOrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Orders.AsQueryable();
            if (request.CustomerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == request.CustomerId.Value);
            }
            return await query
                .ProjectTo<SalesOrderDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}