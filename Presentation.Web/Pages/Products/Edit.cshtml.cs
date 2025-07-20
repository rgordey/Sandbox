using Application;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Products
{
    public class EditModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public UpdateProductCommand Command { get; set; } = new UpdateProductCommand(Guid.Empty, string.Empty, 0, new List<ProductVendorDto>());

        public List<VendorDto> VendorList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var productDto = await _sender.Send(new GetProductQuery(id));
            if (productDto == null)
            {
                return NotFound();
            }

            Command = new UpdateProductCommand(id, productDto.Name, productDto.BasePrice, productDto.Vendors.Select(v => new ProductVendorDto
            {
                VendorId = v.Id,
                VendorPrice = v.VendorPrice,
                StockQuantity = v.StockQuantity
            }).ToList());

            VendorList = await _sender.Send(new GetVendorsQuery());

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                VendorList = await _sender.Send(new GetVendorsQuery());
                return Page();
            }

            await _sender.Send(Command);
            return RedirectToPage("Details", new { Command.Id });
        }
    }
}