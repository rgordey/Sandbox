using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands
{
    public class UpdateCustomerCommand : ICommand<bool>
    {
        public CustomerMetaDto Customer { get; set; } = null!;
    }

    public class UpdateCustomerCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateCustomerCommand, bool>
    {
        public async Task<bool> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await context.Customers
                .AsTracking()
                .FirstOrDefaultAsync(u => u.Id == command.Customer.Id, cancellationToken);

            if (customer == null)
                throw new Exception($"{nameof(Customer)} not found with Id of {command.Customer.Id}"); 

            mapper.Map(command.Customer, customer);

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}