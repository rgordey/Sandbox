// Presentation.Web/Pages/Orders/DetailsModel.cs
using Application;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Orders
{
    public class DetailsModel(IMediator mediator) : PageModel
    {
        public OrderDto Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            Order = await mediator.Send(new GetOrderQuery { OrderId = id });
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}