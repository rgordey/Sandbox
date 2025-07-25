using Application.Abstractions;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrdersDataTableQuery : IQuery<DataTableResponse<SalesOrderDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetOrdersDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetOrdersDataTableQuery, DataTableResponse<SalesOrderDto>>
    {
        public async Task<DataTableResponse<SalesOrderDto>> Handle(GetOrdersDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Orders
                .Include(o => o.Customer) // Ensure Customer is loaded for Name/Email
                .AsNoTracking()
                .AsQueryable();

            var totalRecords = await query.CountAsync(cancellationToken);

            // Global search: Trim, lowercase, match on OrderDate, TotalAmount, or Customer Name/Email
            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                var collation = "SQL_Latin1_General_CP1_CI_AS";
                query = query.Where(o =>
                    o.OrderDate.ToString("yyyy-MM-dd").ToLower().Contains(search) ||
                    o.TotalAmount.ToString().ToLower().Contains(search) ||
                    (o.Customer != null &&
                     (EF.Functions.Collate(o.Customer.Name.Trim().ToLower(), collation).Contains(search) ||
                      EF.Functions.Collate(o.Customer.Email.ToLower(), collation).Contains(search))));
            }

            var filteredRecords = await query.CountAsync(cancellationToken);

            // Sorting with mapping for custom columns
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                var isAsc = request.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);
                query = request.SortColumn.ToLowerInvariant() switch
                {
                    "customerfullname" or "customer.name" => isAsc ? query.OrderBy(o => o.Customer.Name) : query.OrderByDescending(o => o.Customer.Name),
                    "orderdate" => isAsc ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate),
                    "totalamount" => isAsc ? query.OrderBy(o => o.TotalAmount) : query.OrderByDescending(o => o.TotalAmount),
                    _ => query.OrderBy(o => o.OrderDate)  // Default sort by OrderDate asc
                };
            }
            else
            {
                // Default sort by OrderDate asc
                query = query.OrderBy(o => o.OrderDate);
            }

            // Paging and projection
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<SalesOrderDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<SalesOrderDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}