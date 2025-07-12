using Application;
using Application.Features.Customers.Commands;
using Application.Validators;
using FluentValidation.TestHelper;

namespace ApplicationTests
{
    public class ValidationTests
    {
        private readonly UpdateCustomerCommandValidator _validator;

        public ValidationTests()
        {
            _validator = new UpdateCustomerCommandValidator();
        }

        [Fact]
        public async Task Validate_ValidCustomer_ReturnsSuccess()
        {
            var command = new UpdateCustomerCommand
            {
                Customer = new CustomerDto
                {
                    Id = Guid.NewGuid(),
                    FullName = "John Doe",
                    Email = "Test@Test.com"
                }
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_InvalidCustomerId_ReturnsError()
        {
            var command = new UpdateCustomerCommand
            {
                Customer = new CustomerDto
                {
                    Id = Guid.Empty,
                    FullName = "John Doe",
                    Email = "Test@Test.com"
                }
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Customer.Id)
                .WithErrorMessage("Customer Id must be valid.");
        }
    }
}
