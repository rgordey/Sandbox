using Application.Features.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Vendors
{
    public class IndexModel(ISender sender) : PageModel
    {
        private readonly ISender _sender = sender;

        public async Task<IActionResult> OnPostDataTableAsync()
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault() ?? "0";
            var sortColumn = Request.Form["columns[" + sortColumnIndex + "][data]"].FirstOrDefault() ?? "name";
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";
            var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";

            sortColumn = sortColumn.ToLower() switch
            {
                "name" => "Name",
                "contactemail" => "ContactEmail",
                "address.street" => "Address.Street",
                "address.city" => "Address.City",
                "address.state" => "Address.State",
                "address.zipcode" => "Address.ZipCode",
                "address.country" => "Address.Country",
                _ => "Name"
            };

            var query = new GetVendorsDataTableQuery
            {
                Draw = draw,
                Start = start,
                Length = length,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                SearchValue = searchValue
            };

            var response = await _sender.Send(query);

            return new JsonResult(response);
        }
    }
}