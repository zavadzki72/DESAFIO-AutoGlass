namespace Produtos.Domain.Model.ApiContracts
{
    public abstract class PaginatedOrderedRequestForFilter
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public Dictionary<string, string> FieldOrders { get; set; } = new();
    }
}
