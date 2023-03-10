// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Produtos.Infra.SqlServer;

#nullable disable

namespace Produtos.Infra.SqlServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Produtos.Domain.Model.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<DateTime>("ManufacturingDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("manufacturing_date");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int")
                        .HasColumnName("supplier_id");

                    b.Property<DateTime>("ValidDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("valid_date");

                    b.HasKey("Id")
                        .HasName("pk_product");

                    b.HasIndex("Description")
                        .HasDatabaseName("ix_product_description");

                    b.HasIndex("SupplierId")
                        .HasDatabaseName("ix_product_supplier_id");

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("Produtos.Domain.Model.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)")
                        .HasColumnName("cnpj");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_supplier");

                    b.HasIndex("Cnpj")
                        .IsUnique()
                        .HasDatabaseName("ix_supplier_cnpj");

                    b.ToTable("supplier", (string)null);
                });

            modelBuilder.Entity("Produtos.Domain.Model.Entities.Product", b =>
                {
                    b.HasOne("Produtos.Domain.Model.Entities.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_supplier_supplier_id");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Produtos.Domain.Model.Entities.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
