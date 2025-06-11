using Application.Core;
using Application.Core.Abstractions;
using AutoMapper;
using Azure.Core;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions
{
    public class UpdateCustomerCommand : ICommand<bool>
    {
        public CustomerDto Customer { get; set; }
    }

    public class UpdateCustomerCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateCustomerCommand, bool>
    {
        public async Task<bool> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            // Fetch the entity with tracking
            var customer = await context.Customers
                .AsTracking()
                .FirstOrDefaultAsync(u => u.Id == command.Customer.Id, cancellationToken);

            if (customer == null)
                throw new Exception("User not found");

            // Map the command to the entity
            mapper.Map(command.Customer, customer);

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
