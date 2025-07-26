using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Vendors.Commands
{
    public sealed record UpdateVendorCommand(
            Guid Id,
            string Name,
            string ContactEmail,
            AddressDto Address
        ) : ICommand<Unit>; // Changed Nullable to Unit

    internal sealed class UpdateVendorCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateVendorCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await context.Vendors.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException($"Vendor with ID {request.Id} not found.");

            mapper.Map(request, vendor); // Map updated fields
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}