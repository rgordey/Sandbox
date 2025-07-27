using Application;
using Application.Features.Products.Commands;
using Application.Features.Vendors.Queries;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Products
{
    public sealed class CreateModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public CreateProductCommand Command { get; set; } = new(string.Empty, 0, 0, WeightUnit.Kg, 0, 0, 0, DimensionUnit.Cm, new List<ProductVendorDto>());

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Guid id;
            try
            {
                id = await _sender.Send(Command);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }                
                return Page();
            }

            return RedirectToPage("Details", new { id });
        }
    }
}