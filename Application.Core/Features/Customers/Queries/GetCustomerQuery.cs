using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Queries
{
    public sealed class GetCustomerQuery : IQuery<CustomerDto?>
    {
        public Guid CustomerId { get; set; }
    }

    internal sealed class GetCustomerQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCustomerQuery, CustomerDto?>
    {
        public async Task<CustomerDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers
                .Where(x => x.Id == request.CustomerId)
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
