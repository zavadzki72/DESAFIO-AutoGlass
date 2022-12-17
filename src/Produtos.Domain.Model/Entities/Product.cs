namespace Produtos.Domain.Model.Entities
{
    public class Product : BaseEntity
    {
        public Product(string description, bool isActive, DateTime manufacturingDate, DateTime validDate, int supplierId)
        {
            Description = description;
            IsActive = isActive;
            ManufacturingDate = manufacturingDate;
            ValidDate = validDate;
            SupplierId = supplierId;
        }

        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime ManufacturingDate { get; private set; }
        public DateTime ValidDate { get; private set; }
        public int SupplierId { get; private set; }

        public virtual Supplier? Supplier { get; set; }
    }
}
