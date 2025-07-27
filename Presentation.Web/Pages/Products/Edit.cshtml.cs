using Application;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Vendors.Queries;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Web.Pages.Products
{
    public class EditModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        [BindProperty]
        public UpdateProductCommand Command { get; set; } = new(Guid.Empty, string.Empty, 0, 0, WeightUnit.Kg, 0, 0, 0, DimensionUnit.Cm, new List<ProductVendorDto>());

        public List<VendorDto> VendorList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var productDto = await _sender.Send(new GetProductQuery(id));
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
                productDto.Vendors.Select(v => new ProductVendorDto
                {
                    VendorId = v.Id,
                    VendorPrice = v.VendorPrice,
                    StockQuantity = v.StockQuantity
                }).ToList()
            );

            VendorList = await _sender.Send(new GetVendorsQuery());

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _sender.Send(Command);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors) 
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                VendorList = await _sender.Send(new GetVendorsQuery());
                return Page();
            }

            
            return RedirectToPage("Details", new { Command.Id });
        }
    }
}