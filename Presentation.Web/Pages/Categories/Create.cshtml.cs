using Application.Features.Categories.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Categories
{
    public class CreateModel(ISender sender) : PageModel
    {
        

        [BindProperty]
        public CreateCategoryCommand Command { get; set; } = new("", null);

        public IActionResult OnGet()
        {
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
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
