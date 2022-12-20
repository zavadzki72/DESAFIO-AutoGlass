using Produtos.Domain.Model;

namespace Produtos.Domain.Products
{
    public abstract class ProductCommand<T> : Command<T>
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ValidDate { get; set; }
        public int? SupplierId { get; set; }
    }

    public abstract class ProductCommand : Command
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ValidDate { get; set; }
        public int? SupplierId { get; set; }
    }
}
