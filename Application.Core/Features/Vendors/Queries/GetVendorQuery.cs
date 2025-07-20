using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vendors.Queries
{
    public sealed record GetVendorQuery(Guid Id) : IQuery<VendorDto>;

    internal sealed class GetVendorQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetVendorQuery, VendorDto>
    {
        public async Task<VendorDto> Handle(GetVendorQuery request, CancellationToken cancellationToken)
        {
            var vendor = await context.Vendors                
                .Where(v => v.Id == request.Id)
                .ProjectTo<VendorDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (vendor == null)
            {
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found.");
            }

            return vendor;
        }
    }
}