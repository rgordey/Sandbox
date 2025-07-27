using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Categories
{
    public class EditModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public UpdateCategoryCommand Command { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var category = await _sender.Send(new GetCategoryByIdQuery(id));
            if (category == null)
            {
                return NotFound();
            }

            Command = new UpdateCategoryCommand(id, category.Name, category.ParentId);

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