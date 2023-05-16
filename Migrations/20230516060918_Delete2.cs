using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarkLibCW.Migrations
{
    /// <inheritdoc />
    public partial class Delete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Librarians");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Subscribers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Librarians",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Librarians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
