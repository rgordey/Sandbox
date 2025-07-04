﻿using Application.Core;
using Application.Core.Abstractions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Transactions
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
