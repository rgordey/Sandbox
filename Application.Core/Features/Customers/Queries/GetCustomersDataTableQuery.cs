using Application.Abstractions;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;



namespace Application.Features.Customers.Queries
{
    public sealed class GetCustomersDataTableQuery : IQuery<DataTableResponse<CustomerListDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetCustomersDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCustomersDataTableQuery, DataTableResponse<CustomerListDto>>
    {
        public async Task<DataTableResponse<CustomerListDto>> Handle(GetCustomersDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Customers.AsNoTracking().AsQueryable();

            var totalRecords = await query.CountAsync(cancellationToken);

            // Global search: Enhanced to include type-specific name fields in addition to common Name and Email
            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                var collation = "SQL_Latin1_General_CP1_CI_AS";
                query = query.Where(c =>
                    EF.Functions.Collate(c.Name.Trim().ToLower(), collation).Contains(search) ||
                    EF.Functions.Collate(c.Email.ToLower(), collation).Contains(search) ||
                    (EF.Property<string>(c, "CustomerType") == "Residential" &&
                        (EF.Functions.Collate((EF.Property<string>(c, "FirstName") ?? "").Trim().ToLower(), collation).Contains(search) ||
                         EF.Functions.Collate((EF.Property<string>(c, "LastName") ?? "").Trim().ToLower(), collation).Contains(search))) ||
                    (EF.Property<string>(c, "CustomerType") == "Corporate" &&
                        EF.Functions.Collate((EF.Property<string>(c, "CompanyName") ?? "").Trim().ToLower(), collation).Contains(search)) ||
                    (EF.Property<string>(c, "CustomerType") == "Government" &&
                        EF.Functions.Collate((EF.Property<string>(c, "AgencyName") ?? "").Trim().ToLower(), collation).Contains(search))
                );
            }

            var filteredRecords = await query.CountAsync(cancellationToken);

            // Sorting: Use switch for whitelisted columns, supporting common, discriminator, and derived properties
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                var isAsc = request.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);
                query = request.SortColumn.ToLowerInvariant() switch
                {
                    "fullname" or "name" => isAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                    "email" => isAsc ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                    "phonenumber" => isAsc ? query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber),
                    "createdat" => isAsc ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt),
                    "customertype" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "CustomerType")) : query.OrderByDescending(c => EF.Property<string>(c, "CustomerType")),
                    "firstname" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "FirstName")) : query.OrderByDescending(c => EF.Property<string>(c, "FirstName")),
                    "lastname" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "LastName")) : query.OrderByDescending(c => EF.Property<string>(c, "LastName")),
                    "companyname" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "CompanyName")) : query.OrderByDescending(c => EF.Property<string>(c, "CompanyName")),
                    "industry" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "Industry")) : query.OrderByDescending(c => EF.Property<string>(c, "Industry")),
                    "agencyname" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "AgencyName")) : query.OrderByDescending(c => EF.Property<string>(c, "AgencyName")),
                    "department" => isAsc ? query.OrderBy(c => EF.Property<string>(c, "Department")) : query.OrderByDescending(c => EF.Property<string>(c, "Department")),
                    _ => query.OrderBy(c => c.Name)  // Default sort
                };
            }

            // Paging and projection
            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<CustomerListDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<CustomerListDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}
