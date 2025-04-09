using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddTaiKhoanToHoaDonBanOnline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline");
        }
    }
}
