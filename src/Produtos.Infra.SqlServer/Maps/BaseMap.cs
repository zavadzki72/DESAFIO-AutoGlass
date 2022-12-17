using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Produtos.Domain.Model.Entities;

namespace Produtos.Infra.SqlServer.Maps
{
    public class BaseMap<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public BaseMap(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
