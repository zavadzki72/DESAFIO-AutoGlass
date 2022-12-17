using System.Text.Json.Serialization;

namespace Produtos.Domain.Model.ApiContracts
{
    public abstract class PaginatedResult
    {
        public int CurrentPage { get; protected set; }
        public int TotalItems { get; protected set; }
        public int TotalPages { get; protected set; }
    }

    public class PaginatedResult<TData> : PaginatedResult
        where TData : class
    {
        public PaginatedResult(TData data, int currentPage, int totalItems, int sizePage)
        {
            Data = data;
            CurrentPage = currentPage;
            TotalItems = totalItems;

            TotalPages = totalItems / sizePage;
        }

        public TData Data { get; private set; }

    }

    public abstract class PaginatedOrderedResult
    {
        protected PaginatedOrderedResult(long defaultOrderId)
        {
            DefaultOrderId = defaultOrderId;
        }

        [JsonIgnore]
        public long DefaultOrderId { get; set; }
    }
}
