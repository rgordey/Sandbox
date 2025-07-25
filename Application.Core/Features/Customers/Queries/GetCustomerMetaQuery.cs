using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Queries
{
    public sealed class GetCustomerMetaQuery : IQuery<CustomerMetaDto?>
    {
        public Guid CustomerId { get; set; }
    }

    internal sealed class GetCustomerMetaQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCustomerMetaQuery, CustomerMetaDto?>
    {
        public async Task<CustomerMetaDto?> Handle(GetCustomerMetaQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers
                .Where(x => x.Id == request.CustomerId)
                .ProjectTo<CustomerMetaDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
