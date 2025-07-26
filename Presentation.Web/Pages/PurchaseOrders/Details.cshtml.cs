using Application;
using Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.PurchaseOrders
{
    public class DetailsModel(ISender mediator) : PageModel
    {
        public PurchaseOrderDto? PurchaseOrder { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            PurchaseOrder = await mediator.Send(new GetPurchaseOrderQuery { PurchaseOrderId = id });
            if (PurchaseOrder == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}