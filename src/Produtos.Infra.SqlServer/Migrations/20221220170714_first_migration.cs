using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Produtos.Infra.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isactive = table.Column<bool>(name: "is_active", type: "bit", nullable: false),
                    manufacturingdate = table.Column<DateTime>(name: "manufacturing_date", type: "datetime2", nullable: false),
                    validdate = table.Column<DateTime>(name: "valid_date", type: "datetime2", nullable: false),
                    supplierid = table.Column<int>(name: "supplier_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_supplier_supplier_id",
                        column: x => x.supplierid,
                        principalTable: "supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_description",
                table: "product",
                column: "description");

            migrationBuilder.CreateIndex(
                name: "ix_product_supplier_id",
                table: "product",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "ix_supplier_cnpj",
                table: "supplier",
                column: "cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "supplier");
        }
    }
}
