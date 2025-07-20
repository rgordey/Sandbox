using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Vendors.Commands
{
    public sealed record CreateVendorCommand(
        string Name,
        string ContactEmail,
        AddressDto Address
    ) : IRequest<Guid>;

    internal sealed class CreateVendorCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<CreateVendorCommand, Guid>
    {
        public async Task<Guid> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = mapper.Map<Vendor>(request);
            vendor.Id = Guid.NewGuid();

            context.Vendors.Add(vendor);
            await context.SaveChangesAsync(cancellationToken);

            return vendor.Id;
        }
    }

    
}