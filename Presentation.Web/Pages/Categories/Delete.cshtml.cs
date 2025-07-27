using Application.Features.Categories.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Categories
{
    public class DeleteModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        public Guid Id { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Id = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _sender.Send(new DeleteCategoryCommand(id));
            return RedirectToPage("Index");
        }
    }
}