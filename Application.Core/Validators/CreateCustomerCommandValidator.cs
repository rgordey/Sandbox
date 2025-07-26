using Application.Features.Customers.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            // Common validations
            RuleFor(x => x.CustomerType)
                .NotEmpty().WithMessage("Customer Type is required.")
                .Must(type => new[] { "Residential", "Corporate", "Government" }.Contains(type))
                .WithMessage("Customer Type must be one of: Residential, Corporate, Government.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone Number cannot exceed 20 characters.");

            RuleFor(x => x.MailingAddress)
                .SetValidator(new AddressDtoValidator()!);

            RuleFor(x => x.ShippingAddress)
                .SetValidator(new AddressDtoValidator()!);

            // Residential-specific validations
            When(x => x.CustomerType == "Residential", () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First Name is required for Residential customers.")
                    .MaximumLength(25).WithMessage("First Name cannot exceed 25 characters.");

                RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Last Name is required for Residential customers.")
                    .MaximumLength(25).WithMessage("Last Name cannot exceed 25 characters.");

                // IsSeniorDiscountEligible is bool, no specific validation needed beyond default
            });

            // Corporate-specific validations
            When(x => x.CustomerType == "Corporate", () =>
            {
                RuleFor(x => x.CompanyName)
                    .NotEmpty().WithMessage("Company Name is required for Corporate customers.")
                    .MaximumLength(100).WithMessage("Company Name cannot exceed 100 characters.");

                RuleFor(x => x.Industry)
                    .NotEmpty().WithMessage("Industry is required for Corporate customers.")
                    .MaximumLength(50).WithMessage("Industry cannot exceed 50 characters.");

                RuleFor(x => x.EmployeeCount)
                    .GreaterThan(0).WithMessage("Employee Count must be greater than 0 for Corporate customers.");
            });

            // Government-specific validations
            When(x => x.CustomerType == "Government", () =>
            {
                RuleFor(x => x.AgencyName)
                    .NotEmpty().WithMessage("Agency Name is required for Government customers.")
                    .MaximumLength(100).WithMessage("Agency Name cannot exceed 100 characters.");

                RuleFor(x => x.Department)
                    .NotEmpty().WithMessage("Department is required for Government customers.")
                    .MaximumLength(50).WithMessage("Department cannot exceed 50 characters.");

                // IsFederal is bool, no specific validation needed beyond default
            });
        }
    }
}