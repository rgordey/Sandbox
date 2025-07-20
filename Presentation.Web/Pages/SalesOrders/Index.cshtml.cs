// Presentation.Web/Pages/Orders/Index.cshtml.cs
using Application;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.SalesOrders
{
    public class IndexModel(ISender sender) : PageModel
    {
        public List<SalesOrderDto> Orders { get; set; } = new List<SalesOrderDto>();
        public int RecordsTotal { get; set; }

        public async Task OnGetAsync()
        {
            var initialQuery = new GetOrdersDataTableQuery
            {
                Draw = 1,
                Start = 0,
                Length = 10,
                SortColumn = "OrderDate",
                SortDirection = "asc",
                SearchValue = string.Empty
            };

            var response = await sender.Send(initialQuery);
            Orders = response.Data;
            RecordsTotal = response.RecordsTotal;
        }

        public async Task<IActionResult> OnPostDataTableAsync()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault() ?? "0";
            var sortColumn = Request.Form["columns[" + sortColumnIndex + "][data]"].FirstOrDefault() ?? "OrderDate";
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";
            var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // Map DataTables column names to entity-sortable names (case-insensitive)
            sortColumn = sortColumn.ToLower() switch
            {
                "customerfullname" => "CustomerFullName",
                "orderdate" => "OrderDate",
                "totalamount" => "TotalAmount",
                _ => "OrderDate" // Default
            };

            var query = new GetOrdersDataTableQuery
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