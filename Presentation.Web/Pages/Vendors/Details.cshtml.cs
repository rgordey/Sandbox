using Application;
using Application.Features.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Vendors
{
    public class DetailsModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        public VendorDto? Vendor { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Vendor = await _sender.Send(new GetVendorQuery(id));
            if (Vendor == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}