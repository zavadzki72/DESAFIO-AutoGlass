namespace Produtos.Domain.Model
{
    public class PagedRepositoryResponse<TData>
    {
        public PagedRepositoryResponse(List<TData> data, int countData)
        {
            Data = data;
            CountData = countData;
        }

        public List<TData> Data { get; private set; }
        public int CountData { get; private set; }
    }
}
