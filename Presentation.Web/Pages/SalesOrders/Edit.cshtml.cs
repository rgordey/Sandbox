// Presentation.Web/Pages/Orders/EditModel.cs
using Application;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Application.Features.Products.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.SalesOrders
{
    public class EditModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public UpdateOrderCommand Order { get; set; } = new();

        public List<ProductDto> Products { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var fetchedOrder = await mediator.Send(new GetOrderQuery { OrderId = id });
            if (fetchedOrder == null)
            {
                return NotFound();
            }

            Order = new UpdateOrderCommand
            {
                OrderId = fetchedOrder.Id,
                OrderDate = fetchedOrder.OrderDate,
                TotalAmount = fetchedOrder.TotalAmount,
                OrderDetails = fetchedOrder.OrderDetails.Any() ? fetchedOrder.OrderDetails.ToList() : new List<SalesOrderDetailDto>()
            };

            Products = await mediator.Send(new GetProductsQuery());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Reuse Products from OnGet if not modified (optional optimization)
                if (Products == null || !Products.Any())
                {
                    Products = await mediator.Send(new GetProductsQuery());
                }

                await mediator.Send(Order);
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