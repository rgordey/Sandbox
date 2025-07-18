﻿using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Queries
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
