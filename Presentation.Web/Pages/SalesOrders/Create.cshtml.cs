using Application;
using Application.Features.Customers.Queries;
using Application.Features.Orders.Commands;
using Application.Features.Products.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.SalesOrders
{
    public class CreateModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public CreateOrderCommand Order { get; set; } = new() { OrderDetails = new() { new SalesOrderDetailDto() } };

        public List<CustomerDto> Customers { get; set; } = new();
        public List<ProductDto> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Customers = await mediator.Send(new GetCustomersQuery());
            Products = await mediator.Send(new GetProductsQuery());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Customers = await mediator.Send(new GetCustomersQuery());
            Products = await mediator.Send(new GetProductsQuery());
            try
            {
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