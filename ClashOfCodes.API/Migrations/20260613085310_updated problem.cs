using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashOfCodes.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedproblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiddenTestCaseJson",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "TestCaseJson",
                table: "Problems",
                newName: "HiddenTestCasesJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HiddenTestCasesJson",
                table: "Problems",
                newName: "TestCaseJson");

            migrationBuilder.AddColumn<string>(
                name: "HiddenTestCaseJson",
                table: "Problems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
