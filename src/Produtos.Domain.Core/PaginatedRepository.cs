using Microsoft.EntityFrameworkCore;
using Produtos.Domain.Model;
using Produtos.Domain.Model.ApiContracts;

namespace Produtos.Domain.Core
{
    public static class PaginatedRepository
    {
        public static async Task<PagedRepositoryResponse<TModel>> GetPaginatedResult<TFilter, TModel>(this IQueryable<TModel> query, TFilter filter)
            where TModel : PaginatedOrderedResult
            where TFilter : PaginatedOrderedRequestForFilter
        {
            var count = await query.CountAsync();

            if (!filter.FieldOrders.Any())
            {
                filter.FieldOrders.Add(nameof(PaginatedOrderedResult.DefaultOrderId), "asc");
            }

            var orderedQueryable = query.OrderByDictionary(filter.FieldOrders);

            var pagedQuery = orderedQueryable
                .Skip(filter.Page * filter.Size)
                .Take(filter.Size);

            var result = await pagedQuery.ToListAsync();

            return new PagedRepositoryResponse<TModel>(result, count);
        }
    }
}
