namespace Produtos.Domain.Model.Entities
{
    public class Product : BaseEntity
    {
        public Product(string description, DateTime manufacturingDate, DateTime validDate)
        {
            Description = description;
            ManufacturingDate = manufacturingDate;
            ValidDate = validDate;

            IsActive = true;
        }

        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime ManufacturingDate { get; private set; }
        public DateTime ValidDate { get; private set; }
        public int SupplierId { get; private set; }

        public virtual Supplier Supplier { get; set; }

        public void SetSupplier(Supplier supplier)
        {
            Supplier = supplier;
        }

        public void LogicalDelete()
        {
            IsActive = false;
        }

        public void Edit(string? description, DateTime? manufacturingDate, DateTime? validDate)
        {
            if(!string.IsNullOrWhiteSpace(description))
                Description = description;
            
            if(manufacturingDate.HasValue)
                ManufacturingDate = manufacturingDate.Value;
            
            if(validDate.HasValue)
                ValidDate = validDate.Value;
        }
    }
}
