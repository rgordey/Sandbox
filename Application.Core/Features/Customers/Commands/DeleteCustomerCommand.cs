using Application.Abstractions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands
{
    public sealed class DeleteCustomerCommand : ICommand<Unit>
    {
        public Guid CustomerId { get; set; }
    }

    internal sealed class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public DeleteCustomerCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand command, CancellationToken ct)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == command.CustomerId, ct);

            if (customer == null)
            {
                throw new Exception("Customer not found"); // Or custom NotFoundException
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}