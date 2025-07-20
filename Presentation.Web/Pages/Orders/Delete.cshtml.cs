// Presentation.Web/Pages/Orders/DeleteModel.cs
using Application;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Orders
{
    public class DeleteModel(IMediator mediator) : PageModel
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            try
            {
                await mediator.Send(new DeleteOrderCommand { OrderId = id });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                Order = await mediator.Send(new GetOrderQuery { OrderId = id });
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}