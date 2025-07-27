using Application;
using Application.Features.Categories.Queries;
using Application.Features.Products.Commands;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Web.Pages.Products
{
    public sealed class CreateModel(ISender sender) : PageModel
    {
        [BindProperty]
        public CreateProductCommand Command { get; set; } = new(string.Empty, 0, 0, WeightUnit.Kg, 0, 0, 0, DimensionUnit.Cm, null, new List<ProductVendorDto>());

        public List<SelectListItem> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await sender.Send(new GetCategoriesQuery());
            Categories = GetCategorySelectList(categories, null).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Guid id;
            try
            {
                id = await sender.Send(Command);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                var categories = await sender.Send(new GetCategoriesQuery());
                Categories = GetCategorySelectList(categories, Command.CategoryId).ToList();
                return Page();
            }
            
            return RedirectToPage("Details", new { id });
        }

        private IEnumerable<SelectListItem> GetCategorySelectList(List<CategoryDto> categories, Guid? selectedId, Guid? parentId = null, string prefix = "")
        {
            var list = new List<SelectListItem>();

            var childCategories = categories.Where(c => c.ParentId == parentId).OrderBy(c => c.Name);

            foreach (var category in childCategories)
            {
                list.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = prefix + category.Name,
                    Selected = category.Id == selectedId
                });

                list.AddRange(GetCategorySelectList(categories, selectedId, category.Id, prefix + "-- "));
            }

            return list;
        }
    }
}