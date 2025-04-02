using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class themthanhtienphieudattruoc1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThanhTien",
                table: "ChiTietPhieuDatTruoc");

            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTien",
                table: "PhieuDatTruoc",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThanhTien",
                table: "PhieuDatTruoc");

            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTien",
                table: "ChiTietPhieuDatTruoc",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
