using Application.Features.Customers.Commands;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Customer mappings (base to DTO)
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore())
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

            CreateMap<Customer, CustomerMetaDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => EF.Property<string>(src, "CustomerType")))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
                // Ignore type-specific properties in base to prevent translation failures
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.IsSeniorDiscountEligible, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeCount, opt => opt.Ignore())
                .ForMember(dest => dest.Industry, opt => opt.Ignore())
                .ForMember(dest => dest.AgencyName, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.IsFederal, opt => opt.Ignore());

            CreateMap<ResidentialCustomer, CustomerMetaDto>()
                .IncludeBase<Customer, CustomerMetaDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IsSeniorDiscountEligible, opt => opt.MapFrom(src => src.IsSeniorDiscountEligible))
                // Optionally override CustomerType if needed, but base mapping handles it
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => "Residential"));

            CreateMap<CorporateCustomer, CustomerMetaDto>()
                .IncludeBase<Customer, CustomerMetaDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.EmployeeCount))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry))
                // Optionally override CustomerType if needed, but base mapping handles it
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => "Corporate"));

            CreateMap<GovernmentCustomer, CustomerMetaDto>()
                .IncludeBase<Customer, CustomerMetaDto>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.AgencyName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.IsFederal, opt => opt.MapFrom(src => src.IsFederal))
                // Optionally override CustomerType if needed, but base mapping handles it
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => "Government"));

            CreateMap<CustomerMetaDto, Customer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                // Ignore properties that shouldn't be updated or are auto-managed
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<CustomerMetaDto, ResidentialCustomer>()
                .IncludeBase<CustomerMetaDto, Customer>()
                // For Residential, set Name via FirstName/LastName setters
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IsSeniorDiscountEligible, opt => opt.MapFrom(src => src.IsSeniorDiscountEligible));

            CreateMap<CustomerMetaDto, CorporateCustomer>()
                .IncludeBase<CustomerMetaDto, Customer>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.EmployeeCount))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry));

            CreateMap<CustomerMetaDto, GovernmentCustomer>()
                .IncludeBase<CustomerMetaDto, Customer>()
                .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.AgencyName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.IsFederal, opt => opt.MapFrom(src => src.IsFederal));

            // Polymorphic mappings for derived types to DTO (if additional properties needed; extend DTO if required)
            CreateMap<ResidentialCustomer, CustomerDto>()
                .IncludeBase<Customer, CustomerDto>()
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore());

            CreateMap<CorporateCustomer, CustomerDto>()
                .IncludeBase<Customer, CustomerDto>()
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore());

            CreateMap<GovernmentCustomer, CustomerDto>()
                .IncludeBase<Customer, CustomerDto>()
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore());

            // Command to Entity (assuming CreateCustomerCommand has all possible properties; map conditionally)
            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Auto-set in entity
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));

            // If using reverse mapping (DTO to Entity), define polymorphic for each type to avoid issues with type-specific props
            // Note: Use explicit below if needed for updates
            CreateMap<CustomerDto, ResidentialCustomer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())  // Set via First/Last setters
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Contains(' ') ? src.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0] : src.FullName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Contains(' ') ? string.Join(" ", src.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1)) : string.Empty))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.IsSeniorDiscountEligible, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());  // Handle separately

            CreateMap<CustomerDto, CorporateCustomer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeCount, opt => opt.Ignore())
                .ForMember(dest => dest.Industry, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<CustomerDto, GovernmentCustomer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.AgencyName, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.IsFederal, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Customer, CustomerListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => EF.Property<string>(src, "CustomerType")));
        }
    }
}
