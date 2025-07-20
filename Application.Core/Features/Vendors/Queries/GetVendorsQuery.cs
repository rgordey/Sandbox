// Application/Features/Vendors/Queries/GetVendorsQuery.cs (new)
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vendors.Queries
{
    public sealed class GetVendorsQuery : IQuery<List<VendorDto>>
    {
    }

    internal sealed class GetVendorsQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetVendorsQuery, List<VendorDto>>
    {
        public async Task<List<VendorDto>> Handle(GetVendorsQuery request, CancellationToken cancellationToken)
        {
            return await context.Vendors
                .ProjectTo<VendorDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}