using Application.Abstractions;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Categories.Queries
{
    public sealed class GetCategoriesDataTableQuery : IQuery<DataTableResponse<CategoryDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetCategoriesDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCategoriesDataTableQuery, DataTableResponse<CategoryDto>>
    {
        public async Task<DataTableResponse<CategoryDto>> Handle(GetCategoriesDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Categories.AsNoTracking().Include(c => c.Parent).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{search}%"));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var filteredRecords = totalRecords;

            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                var sortExpression = request.SortColumn switch
                {
                    "parentName" => "Parent.Name " + request.SortDirection,
                    _ => request.SortColumn + " " + request.SortDirection
                };
                query = query.OrderBy(sortExpression);
            }

            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<CategoryDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}