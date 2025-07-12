using Application;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public class EditModel(IMediator mediator) : PageModel
    {
        

        [BindProperty]
        public CustomerDto Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var customer = await mediator.Send(new GetCustomerQuery() { CustomerId = id });
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await mediator.Send(new UpdateCustomerCommand() { Customer = Customer });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                Customer = await mediator.Send(new GetCustomerQuery() { CustomerId = Customer.Id });
                return Page();
            }           

            return RedirectToPage("./Index");
        }

        
    }
}
