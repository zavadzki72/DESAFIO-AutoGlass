using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Produtos.Domain.Model.Entities;

namespace Produtos.Infra.SqlServer.Maps
{
    public class ProductMap : BaseMap<Product>
    {
        public ProductMap() : base("product") { }

        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.ManufacturingDate)
                .IsRequired();

            builder.Property(x => x.ValidDate)
                .IsRequired();

            builder.Property(x => x.SupplierId)
                .IsRequired();

            builder.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierId);

            builder.HasIndex(x => x.Description);

            base.Configure(builder);
        }
    }
}
