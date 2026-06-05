using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashOfCodes.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTestCasesJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestCasesJson",
                table: "Problems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestCasesJson",
                table: "Problems");
        }
    }
}
