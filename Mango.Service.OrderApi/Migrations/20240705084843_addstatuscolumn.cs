using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Service.OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class addstatuscolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderHeader");
        }
    }
}
