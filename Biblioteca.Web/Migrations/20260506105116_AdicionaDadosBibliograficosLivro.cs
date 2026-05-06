using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDadosBibliograficosLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnoPublicacao",
                table: "Livros",
                newName: "NumeroPaginas");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPublicacao",
                table: "Livros",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Edicao",
                table: "Livros",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Editora",
                table: "Livros",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_DataPublicacao",
                table: "Livros",
                column: "DataPublicacao");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_Editora",
                table: "Livros",
                column: "Editora");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Livros_DataPublicacao",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Livros_Editora",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "DataPublicacao",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "Edicao",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "Editora",
                table: "Livros");

            migrationBuilder.RenameColumn(
                name: "NumeroPaginas",
                table: "Livros",
                newName: "AnoPublicacao");
        }
    }
}
