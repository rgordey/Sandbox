using Application;
using Application.Features.Vendors.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Vendors
{
    public class CreateModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public CreateVendorCommand Command { get; set; } = new CreateVendorCommand(string.Empty, string.Empty, new AddressDto());

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = await _sender.Send(Command);
            return RedirectToPage("Details", new { id });
        }
    }
}