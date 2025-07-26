// DetailsModel.cs
using Application;
using Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public class DetailsModel(ISender mediator) : PageModel
    {
        public CustomerMetaDto Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var customer = await mediator.Send(new GetCustomerMetaQuery() { CustomerId = id });
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }
    }
}