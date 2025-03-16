using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class Addtaikhoanvathongtin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien",
                column: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "NhanVien",
                column: "ThongTinNguoiDungMaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
