using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Produtos.Domain.Model.Entities;

namespace Produtos.Infra.SqlServer.Maps
{
    public class SupplierMap : BaseMap<Supplier>
    {
        public SupplierMap() : base("supplier") { }

        public override void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.Cnpj)
                .HasMaxLength(14)
                .IsRequired();

            builder.HasIndex(x => x.Cnpj)
                .IsUnique();

            base.Configure(builder);
        }
    }
}
