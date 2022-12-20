using System.Linq.Expressions;

namespace Produtos.Domain.Core
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDictionary<T>(this IQueryable<T> source, Dictionary<string, string> sort)
        {
            var expression = source.Expression;
            int count = 0;

            if (!sort.Any())
            {
                return source;
            }

            foreach (var item in sort)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, item.Key);

                string method = string.Empty;

                if (string.Equals(item.Value, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    method = (count == 0 ? "OrderByDescending" : "ThenByDescending");
                }
                else
                {
                    method = (count == 0 ? "OrderBy" : "ThenBy");
                }

                expression = Expression.Call(typeof(Queryable), method, new Type[] { source.ElementType, selector.Type }, expression, Expression.Quote(Expression.Lambda(selector, parameter)));

                count++;
            }

            return source.Provider.CreateQuery<T>(expression);
        }
    }
}
