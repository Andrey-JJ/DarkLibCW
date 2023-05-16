using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarkLibCW.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateWithList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_CatalogCards_CatalogCardId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CatalogCardId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CatalogCardId",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorCatalogCard",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CatalogCardsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorCatalogCard", x => new { x.AuthorId, x.CatalogCardsId });
                    table.ForeignKey(
                        name: "FK_AuthorCatalogCard_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorCatalogCard_CatalogCards_CatalogCardsId",
                        column: x => x.CatalogCardsId,
                        principalTable: "CatalogCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorCatalogCard_CatalogCardsId",
                table: "AuthorCatalogCard",
                column: "CatalogCardsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorCatalogCard");

            migrationBuilder.AddColumn<int>(
                name: "CatalogCardId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CatalogCardId",
                table: "Authors",
                column: "CatalogCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_CatalogCards_CatalogCardId",
                table: "Authors",
                column: "CatalogCardId",
                principalTable: "CatalogCards",
                principalColumn: "Id");
        }
    }
}
