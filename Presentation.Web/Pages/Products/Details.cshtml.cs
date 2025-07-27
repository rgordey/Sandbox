using Application;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Products
{
    public sealed class DetailsModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        public ProductDto Product { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Product = await _sender.Send(new GetProductQuery(id));
            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}