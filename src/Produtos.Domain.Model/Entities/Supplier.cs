namespace Produtos.Domain.Model.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier() { }

        public Supplier(string description, string cnpj)
        {
            Description = description;
            Cnpj = cnpj;
        }

        public string Description { get; private set; }
        public string Cnpj { get; private set; }

        public virtual List<Product>? Products { get; set; }

        public void Edit(string? description, string? cnpj)
        {
            if (!string.IsNullOrWhiteSpace(description))
                Description = description;
            
            if (!string.IsNullOrWhiteSpace(cnpj))
                Cnpj = cnpj;
        }
    }
}
