using Application.Abstractions;
using Application.Common; // For DataTableResponse
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Vendors.Queries
{
    public sealed class GetVendorsDataTableQuery : IQuery<DataTableResponse<VendorListDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetVendorsDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetVendorsDataTableQuery, DataTableResponse<VendorListDto>>
    {
        public async Task<DataTableResponse<VendorListDto>> Handle(GetVendorsDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Vendors.AsNoTracking().AsQueryable();

            // Global search: Trim, lowercase, match on Name or ContactEmail
            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(v =>
                    EF.Functions.Like(v.Name.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(v.ContactEmail.ToLower(), $"%{search}%"));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var filteredRecords = totalRecords;

            // Sorting with dynamic LINQ (supports nested like Address.Line1)
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                var sortColumn = request.SortColumn.ToLower() switch
                {
                    "name" => "Name",
                    "contactemail" => "ContactEmail",
                    "address.line1" => "Address.Line1",
                    "address.city" => "Address.City",
                    "address.state" => "Address.State",
                    "address.zipcode" => "Address.ZipCode",
                    _ => "Name"
                };

                var sortExpression = $"{sortColumn} {request.SortDirection}";
                query = query.OrderBy(sortExpression);
            }

            // Paging and projection with VendorListDto
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<VendorListDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<VendorListDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}