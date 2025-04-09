using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class ReAdd_HoaDonBanOnlinekkkk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonBanOnline_TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropColumn(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonBanOnline_TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "TaiKhoanNguoiDungMaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "TaiKhoanNguoiDungMaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
