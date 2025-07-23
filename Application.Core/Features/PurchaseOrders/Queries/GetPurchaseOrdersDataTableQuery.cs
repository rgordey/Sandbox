// Application/Features/PurchaseOrders/Queries/GetPurchaseOrdersDataTableQuery.cs
using Application.Abstractions;
using Application.Common; // For DataTableResponse
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.PurchaseOrders.Queries
{
    public sealed class GetPurchaseOrdersDataTableQuery : IQuery<DataTableResponse<PurchaseOrderListDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetPurchaseOrdersDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetPurchaseOrdersDataTableQuery, DataTableResponse<PurchaseOrderListDto>>
    {
        private static readonly Dictionary<string, string> ColumnMap = new()
        {
            { "vendorName", "Vendor.Name" },
            { "orderDate", "OrderDate" },
            { "totalAmount", "TotalAmount" }
        };

        public async Task<DataTableResponse<PurchaseOrderListDto>> Handle(GetPurchaseOrdersDataTableQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = context.PurchaseOrders
                .Include(po => po.Vendor)
                .AsNoTracking();

            var totalRecords = await baseQuery.CountAsync(cancellationToken);

            var query = baseQuery.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(po =>
                    EF.Functions.Like(po.Vendor.Name.ToLower(), $"%{search}%"));
            }

            var filteredRecords = await query.CountAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.SortColumn) && ColumnMap.TryGetValue(request.SortColumn, out var entityColumn))
            {
                var sortExpression = $"{entityColumn} {request.SortDirection}";
                query = query.OrderBy(sortExpression);
            }
            else
            {
                // Default sort, e.g., by OrderDate descending
                query = query.OrderByDescending(po => po.OrderDate);
            }

            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<PurchaseOrderListDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<PurchaseOrderListDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}