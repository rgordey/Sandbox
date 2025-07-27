// Presentation.Web/Pages/Products/Edit.cshtml.cs (updated)
using Application;
using Application.Features.Categories.Queries;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Vendors.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Web.Pages.Products
{
    public class EditModel(ISender sender) : PageModel
    {
        [BindProperty]
        public UpdateProductCommand Command { get; set; } = new(Guid.Empty, string.Empty, 0, 0, WeightUnit.Kg, 0, 0, 0, DimensionUnit.Cm, null, new List<ProductVendorDto>());

        public List<VendorDto> VendorList { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var productDto = await sender.Send(new GetProductByIdQuery(id));
            if (productDto == null)
            {
                return NotFound();
            }

            Command = new UpdateProductCommand(
                id,
                productDto.Name,
                productDto.BasePrice,
                productDto.Weight,
                productDto.WeightUnit,
                productDto.Length,
                productDto.Width,
                productDto.Height,
                productDto.DimensionUnit,
                productDto.CategoryId,
                productDto.Vendors.Select(v => new ProductVendorDto
                {
                    VendorId = v.Id,  // Changed from v.VendorId to v.Id
                    VendorPrice = v.VendorPrice,
                    StockQuantity = v.StockQuantity
                }).ToList()
            );

            VendorList = await sender.Send(new GetVendorsQuery());

            var categories = await sender.Send(new GetCategoriesQuery());
            Categories = GetCategorySelectList(categories, productDto.CategoryId).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                VendorList = await sender.Send(new GetVendorsQuery());
                var categories = await sender.Send(new GetCategoriesQuery());
                Categories = GetCategorySelectList(categories, Command.CategoryId).ToList();
                return Page();
            }

            await sender.Send(Command);
            return RedirectToPage("Details", new { Command.Id });
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