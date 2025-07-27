using Application;
using Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public sealed class IndexModel(ISender sender) : PageModel
    {
        public List<CustomerListDto> Customers { get; set; } = new List<CustomerListDto>();
        public int RecordsTotal { get; set; }

        public async Task OnGetAsync()
        {
            var initialQuery = new GetCustomersDataTableQuery
            {
                Draw = 1, // Arbitrary for initial
                Start = 0,
                Length = 10, // Match default pageLength
                SortColumn = "FullName",
                SortDirection = "asc",
                SearchValue = string.Empty
            };

            var response = await sender.Send(initialQuery);
            Customers = response.Data;
            RecordsTotal = response.RecordsTotal;
        }

        public async Task<IActionResult> OnPostDataTableAsync()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault() ?? "0";
            var sortColumn = Request.Form["columns[" + sortColumnIndex + "][data]"].FirstOrDefault() ?? "FullName";
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";
            var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // Map DataTables column names to entity-sortable names (case-insensitive)
            sortColumn = sortColumn.ToLower() switch
            {
                "fullname" => "FullName", // Triggers special handling in handler
                "email" => "Email",
                _ => "FullName" // Default
            };

            var query = new GetCustomersDataTableQuery
            {
                Draw = draw,
                Start = start,
                Length = length,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                SearchValue = searchValue
            };

            var response = await sender.Send(query);

            return new JsonResult(response);
        }
    }
}
