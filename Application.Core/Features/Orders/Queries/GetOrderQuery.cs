// Application/Features/Orders/Queries/GetOrderQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrderQuery : IQuery<SalesOrderDto?>
    {
        public Guid OrderId { get; set; }
    }

    internal sealed class GetOrderQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrderQuery, SalesOrderDto?>
    {
        public async Task<SalesOrderDto?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            return await context.Orders
                .Where(x => x.Id == query.OrderId)
                .ProjectTo<SalesOrderDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}