using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;


namespace Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string CustomerType { get; set; } = null!; // "Residential", "Corporate", "Government"

        // Common
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public AddressDto? MailingAddress { get; set; }
        public AddressDto? ShippingAddress { get; set; }

        // Residential
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsSeniorDiscountEligible { get; set; }

        // Corporate
        public string? CompanyName { get; set; }
        public int EmployeeCount { get; set; }
        public string? Industry { get; set; }

        // Government
        public string? AgencyName { get; set; }
        public string? Department { get; set; }
        public bool IsFederal { get; set; }
    }

    internal sealed class CreateCustomerCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = request.CustomerType switch
            {
                "Residential" => new ResidentialCustomer
                {
                    FirstName = request.FirstName ?? throw new ArgumentNullException(nameof(request.FirstName)),
                    LastName = request.LastName ?? throw new ArgumentNullException(nameof(request.LastName)),
                    // Name set by setters
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    IsSeniorDiscountEligible = request.IsSeniorDiscountEligible,
                    MailingAddress = mapper.Map<Address>(request.MailingAddress),
                    ShippingAddress = mapper.Map<Address>(request.ShippingAddress)
                },
                "Corporate" => new CorporateCustomer
                {
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CompanyName = request.CompanyName ?? throw new ArgumentNullException(nameof(request.CompanyName)),
                    EmployeeCount = request.EmployeeCount,
                    Industry = request.Industry ?? throw new ArgumentNullException(nameof(request.Industry)),
                    MailingAddress = mapper.Map<Address>(request.MailingAddress),
                    ShippingAddress = mapper.Map<Address>(request.ShippingAddress)
                },
                "Government" => new GovernmentCustomer
                {
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    AgencyName = request.AgencyName ?? throw new ArgumentNullException(nameof(request.AgencyName)),
                    Department = request.Department ?? throw new ArgumentNullException(nameof(request.Department)),
                    IsFederal = request.IsFederal,
                    MailingAddress = mapper.Map<Address>(request.MailingAddress),
                    ShippingAddress = mapper.Map<Address>(request.ShippingAddress)
                },
                _ => throw new InvalidOperationException("Invalid CustomerType")
            };

            customer.Id = Guid.NewGuid(); // Or use DB-generated if applicable

            context.Customers.Add(customer);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<CustomerDto>(customer);
        }
    }
}