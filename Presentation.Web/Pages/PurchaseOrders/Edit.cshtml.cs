using Application;
using Application.Features.Products.Queries;
using Application.Features.PurchaseOrders.Commands;
using Application.Features.PurchaseOrders.Queries;
using Application.Features.Vendors.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.PurchaseOrders
{
    public class EditModel(ISender mediator) : PageModel
    {
        [BindProperty]
        public UpdatePurchaseOrderCommand PurchaseOrder { get; set; } = new();

        public List<VendorDto> Vendors { get; set; } = new();
        public List<ProductDto> Products { get; set; } = new();
        public List<ProductVendorDto> ProductVendors { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var poDto = await mediator.Send(new GetPurchaseOrderQuery { PurchaseOrderId = id });
            if (poDto == null)
            {
                return NotFound();
            }

            PurchaseOrder = new UpdatePurchaseOrderCommand
            {
                Id = poDto.Id,
                VendorId = poDto.VendorId,
                OrderDate = poDto.OrderDate,
                TotalAmount = poDto.TotalAmount,
                OrderDetails = poDto.OrderDetails
            };

            Vendors = await mediator.Send(new GetVendorsQuery());
            Products = await mediator.Send(new GetProductsQuery());
            ProductVendors = await mediator.Send(new GetProductVendorsQuery());
            ViewData["Products"] = Products;
            ViewData["ProductVendors"] = ProductVendors;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Vendors = await mediator.Send(new GetVendorsQuery());
            Products = await mediator.Send(new GetProductsQuery());
            ProductVendors = await mediator.Send(new GetProductVendorsQuery());
            ViewData["Products"] = Products;
            ViewData["ProductVendors"] = ProductVendors;

            try
            {
                await mediator.Send(PurchaseOrder);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}