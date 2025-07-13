using Application;
using Application.Features.Customers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ISender _sender;

        public CreateModel(ISender sender)
        {
            _sender = sender;
        }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sender.Send(Command);

            return RedirectToPage("./Index");
        }
    }
}