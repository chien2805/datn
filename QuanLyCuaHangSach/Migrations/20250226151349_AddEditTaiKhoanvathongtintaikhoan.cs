using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddEditTaiKhoanvathongtintaikhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_TaiKhoanNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.DropColumn(
                name: "MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.AddColumn<string>(
                name: "AnhDaiDien",
                table: "ThongTinNguoiDung",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaTaiKhoan",
                table: "ThongTinNguoiDung",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinNguoiDung_MaTaiKhoan",
                table: "ThongTinNguoiDung",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ThongTinNguoiDung_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "ThongTinNguoiDung",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinNguoiDung_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "ThongTinNguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_ThongTinNguoiDung_MaTaiKhoan",
                table: "ThongTinNguoiDung");

            migrationBuilder.DropColumn(
                name: "AnhDaiDien",
                table: "ThongTinNguoiDung");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "ThongTinNguoiDung");

            migrationBuilder.AddColumn<int>(
                name: "MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoanNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "MaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "MaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung");
        }
    }
}
