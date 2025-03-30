using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHoaDonOnline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDonOnline_HoaDonBanOnline_HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDonOnline_Sach_SachMaSach",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDonOnline_HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDonOnline_SachMaSach",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropColumn(
                name: "HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropColumn(
                name: "SachMaSach",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_MaHoaDon",
                table: "ChiTietHoaDonOnline",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_MaSach",
                table: "ChiTietHoaDonOnline",
                column: "MaSach");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDonOnline_HoaDonBanOnline_MaHoaDon",
                table: "ChiTietHoaDonOnline",
                column: "MaHoaDon",
                principalTable: "HoaDonBanOnline",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDonOnline_Sach_MaSach",
                table: "ChiTietHoaDonOnline",
                column: "MaSach",
                principalTable: "Sach",
                principalColumn: "MaSach",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDonOnline_HoaDonBanOnline_MaHoaDon",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDonOnline_Sach_MaSach",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDonOnline_MaHoaDon",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDonOnline_MaSach",
                table: "ChiTietHoaDonOnline");

            migrationBuilder.AddColumn<int>(
                name: "HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SachMaSach",
                table: "ChiTietHoaDonOnline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_SachMaSach",
                table: "ChiTietHoaDonOnline",
                column: "SachMaSach");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDonOnline_HoaDonBanOnline_HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline",
                column: "HoaDonMaHoaDon",
                principalTable: "HoaDonBanOnline",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDonOnline_Sach_SachMaSach",
                table: "ChiTietHoaDonOnline",
                column: "SachMaSach",
                principalTable: "Sach",
                principalColumn: "MaSach",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
