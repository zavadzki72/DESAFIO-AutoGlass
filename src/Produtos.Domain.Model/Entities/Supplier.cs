namespace Produtos.Domain.Model.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier(string description, decimal cnpj)
        {
            Description = description;
            Cnpj = cnpj;
        }

        public string Description { get; private set; }
        public decimal Cnpj { get; private set; }

        public virtual List<Product>? Products { get; set; }
    }
}
