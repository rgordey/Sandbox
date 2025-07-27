using Application;
using Application.Features.Vendors.Commands;
using Application.Features.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Vendors
{
    public class EditModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public UpdateVendorCommand Command { get; set; } = new UpdateVendorCommand(Guid.Empty, string.Empty, string.Empty, string.Empty, new AddressDto());

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vendorDto = await _sender.Send(new GetVendorQuery(id));
            if (vendorDto == null)
            {
                return NotFound();
            }

            Command = new UpdateVendorCommand(id, vendorDto.Name, vendorDto.ContactEmail, vendorDto.VendorNumber, vendorDto.Address);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sender.Send(Command);
            return RedirectToPage("Index");
        }
    }
}