using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AddressDto? MailingAddress { get; set; }
        public AddressDto? ShippingAddress { get; set; }
    }

    internal sealed class CreateCustomerCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = mapper.Map<Customer>(request);
            customer.Id = Guid.NewGuid(); // Or use DB-generated if applicable

            context.Customers.Add(customer);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<CustomerDto>(customer);
        }
    }
}