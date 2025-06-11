using Application.Core;
using Application.Core.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Transactions
{
    public sealed class GetCustomersQuery : IQuery<List<CustomerDto>>
    {

    }

    internal sealed class GetCustomersQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCustomersQuery, List<CustomerDto>>
    {
        public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
