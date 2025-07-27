// Application.Features.Vendors.Commands/UpdateVendorCommand.cs (updated to force modified state)
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore; // Add this for EntityState
using Microsoft.Extensions.Logging;

namespace Application.Features.Vendors.Commands
{
    public sealed record UpdateVendorCommand(
            Guid Id,
            string Name,
            string ContactEmail,
            AddressDto Address
        ) : ICommand<Unit>;

    internal sealed class UpdateVendorCommandHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<UpdateVendorCommandHandler> logger) : ICommandHandler<UpdateVendorCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await context.Vendors.Where(x => x.Id == request.Id ).AsTracking().FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Vendor with ID {request.Id} not found.");

            logger.LogInformation("Pre-map Address: {@Address}", vendor.Address);
            mapper.Map(request, vendor);
            logger.LogInformation("Post-map Address: {@Address}", vendor.Address);

            

            var rowsAffected = await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Rows affected by save: {RowsAffected}", rowsAffected);

            return Unit.Value;
        }
    }
}