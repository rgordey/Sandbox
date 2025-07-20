using Application;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Products
{
    public class DeleteModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        public ProductDto? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Product = await _sender.Send(new GetProductQuery(id));
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _sender.Send(new DeleteProductCommand(id));
            return RedirectToPage("Index");
        }
    }
}