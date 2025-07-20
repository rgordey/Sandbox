// Application/Features/Orders/Queries/GetOrdersDataTableQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrdersDataTableQuery : IQuery<DataTableResponse<OrderDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    public class DataTableResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; } = new List<T>(); // Use List for serialization
    }

    internal sealed class GetOrdersDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrdersDataTableQuery, DataTableResponse<OrderDto>>
    {
        public async Task<DataTableResponse<OrderDto>> Handle(GetOrdersDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Orders
                .Include(o => o.Customer) // Ensure Customer is loaded for FullName projection
                .AsNoTracking()
                .AsQueryable();

            // Global search: Trim, lowercase, match on OrderDate, TotalAmount, or Customer FullName/Email
            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(o =>
                    o.OrderDate.ToString("yyyy-MM-dd").ToLower().Contains(search) ||
                    o.TotalAmount.ToString().ToLower().Contains(search) ||
                    (o.Customer != null &&
                     (EF.Functions.Collate((o.Customer.FirstName.Trim().ToLower() + " " + o.Customer.LastName.Trim()).ToLower(), "SQL_Latin1_General_CP1_CI_AS").Contains(search) ||
                      EF.Functions.Collate(o.Customer.Email.ToLower(), "SQL_Latin1_General_CP1_CI_AS").Contains(search))));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var filteredRecords = totalRecords;

            // Sorting with mapping for custom columns
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                if (request.SortColumn.Equals("CustomerFullName", StringComparison.OrdinalIgnoreCase))
                {
                    if (request.SortDirection == "asc")
                    {
                        query = query.OrderBy(o => o.Customer != null ? o.Customer.FirstName : "")
                                     .ThenBy(o => o.Customer != null ? o.Customer.LastName : "");
                    }
                    else
                    {
                        query = query.OrderByDescending(o => o.Customer != null ? o.Customer.FirstName : "")
                                     .ThenByDescending(o => o.Customer != null ? o.Customer.LastName : "");
                    }
                }
                else
                {
                    // Handle other sortable fields (OrderDate, TotalAmount)
                    query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
                }
            }
            else
            {
                // Default sort by OrderDate if no column specified
                query = query.OrderBy("OrderDate asc");
            }

            // Paging and projection
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<OrderDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}