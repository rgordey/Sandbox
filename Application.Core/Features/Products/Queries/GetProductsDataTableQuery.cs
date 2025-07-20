using Application.Abstractions;
using Application.Common;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductsDataTableQuery : IQuery<DataTableResponse<ProductDto>>
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
        public string SearchValue { get; set; } = string.Empty;
    }

    internal sealed class GetProductsDataTableQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetProductsDataTableQuery, DataTableResponse<ProductDto>>
    {
        public async Task<DataTableResponse<ProductDto>> Handle(GetProductsDataTableQuery request, CancellationToken cancellationToken)
        {
            var query = context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var search = request.SearchValue.Trim().ToLower();
                query = query.Where(p =>
                    EF.Functions.Like(p.Name.ToLower(), $"%{search}%"));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var filteredRecords = totalRecords;

            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                var sortExpression = $"{request.SortColumn} {request.SortDirection}";
                query = query.OrderBy(sortExpression);
            }

            var data = await query
                .Skip(request.Start)
                .Take(request.Length)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new DataTableResponse<ProductDto>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
        }
    }
}