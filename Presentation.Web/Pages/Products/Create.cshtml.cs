using Application;
using Application.Features.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Products
{
    public class CreateModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public CreateProductCommand Command { get; set; } = new CreateProductCommand(string.Empty, 0, new List<ProductVendorDto>());

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