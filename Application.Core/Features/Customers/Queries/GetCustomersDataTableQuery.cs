using Application.Abstractions;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Customers.Queries
{
    public sealed class GetCustomersDataTableQuery : IQuery<DataTableResponse<CustomerDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    

    internal sealed class GetCustomersDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCustomersDataTableQuery, DataTableResponse<CustomerDto>>
    {
        public async Task<DataTableResponse<CustomerDto>> Handle(GetCustomersDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Customers.AsNoTracking().AsQueryable();

            // Global search: Trim, lowercase, match on trimmed concatenated name or email
            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(c =>
                    EF.Functions.Collate((c.FirstName.Trim().ToLower() + " " + c.LastName.Trim()).ToLower(), "SQL_Latin1_General_CP1_CI_AS").Contains(search) ||
                    EF.Functions.Collate(c.Email.ToLower(), "SQL_Latin1_General_CP1_CI_AS").Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var filteredRecords = totalRecords;

            // Sorting with FullName mapping
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                if (request.SortColumn.Equals("FullName", StringComparison.OrdinalIgnoreCase))
                {
                    if (request.SortDirection == "asc")
                    {
                        query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    }
                    else
                    {
                        query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    }
                }
                else
                {
                    query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
                }
            }

            // Paging and projection (AutoMapper computes FullName, ignores nav props if not loaded
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<CustomerDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}