// Presentation.Web/Pages/PurchaseOrders/CreateModel.cs (new)
using Application;
using Application.Features.PurchaseOrders.Commands;
using Application.Features.Vendors.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.PurchaseOrders
{
    public class CreateModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public CreatePurchaseOrderCommand PurchaseOrder { get; set; } = new();

        public List<VendorDto> Vendors { get; set; } = new();

        public async Task OnGetAsync()
        {
            Vendors = await mediator.Send(new GetVendorsQuery());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Vendors = await mediator.Send(new GetVendorsQuery());
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