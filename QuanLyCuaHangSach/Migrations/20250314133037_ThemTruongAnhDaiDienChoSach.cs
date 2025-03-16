using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class ThemTruongAnhDaiDienChoSach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang",
                column: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang",
                column: "ThongTinNguoiDungMaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang");
        }
    }
}
