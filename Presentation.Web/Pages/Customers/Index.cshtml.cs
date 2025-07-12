using Application;
using Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Web.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ISender _sender;

        public IndexModel(ISender sender)
        {
            _sender = sender;
        }

        public IList<CustomerDto> Customers { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Customers = await _sender.Send(new GetCustomersQuery());
        }
    }
}
