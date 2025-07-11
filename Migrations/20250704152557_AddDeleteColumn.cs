using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeleteFlag",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteFlag",
                table: "Products");
        }
    }
}
