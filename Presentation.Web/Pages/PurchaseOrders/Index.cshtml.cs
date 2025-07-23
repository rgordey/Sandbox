using Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.PurchaseOrders
{
    public class IndexModel(IMediator mediator) : PageModel
    {
        public void OnGet()
        {
            // Data loaded via AJAX for DataTables
        }

        public async Task<IActionResult> OnPostDataTableAsync(int draw, int start, int length, string? sortColumn = null, string? sortDirection = "asc", string? searchValue = null)
        {
            var query = new GetPurchaseOrdersDataTableQuery
            {
                Draw = draw,
                Start = start,
                Length = length,
                SortColumn = sortColumn ?? string.Empty,
                SortDirection = sortDirection ?? "asc",
                SearchValue = searchValue ?? string.Empty
            };

            var result = await mediator.Send(query);
            return new JsonResult(result);
        }
    }
}