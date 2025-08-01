﻿// DeleteModel.cs
using Application;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public sealed class DeleteModel(ISender mediator) : PageModel
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            try
            {
                await mediator.Send(new DeleteCustomerCommand() { CustomerId = id });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                Customer = await mediator.Send(new GetCustomerMetaQuery() { CustomerId = id });
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}