using Application;
using Application.Features.Customers.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public class CreateModel(ISender sender) : PageModel
    {
        [BindProperty]
        public CreateCustomerCommand Command { get; set; } = new();

        public IActionResult OnGet()
        {
            Command.MailingAddress = new AddressDto();
            Command.ShippingAddress = new AddressDto();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            try
            {
                await sender.Send(Command);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }   
            }

            return RedirectToPage("./Index");
        }
    }
}