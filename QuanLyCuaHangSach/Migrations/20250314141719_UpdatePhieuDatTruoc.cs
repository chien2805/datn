using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhieuDatTruoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_KhachHang_KhachHangMaKhachHang",
                table: "PhieuDatTruoc");

            migrationBuilder.DropIndex(
                name: "IX_PhieuDatTruoc_KhachHangMaKhachHang",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKhachHang",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "MaKhachHang",
                table: "PhieuDatTruoc");

            migrationBuilder.AddColumn<int>(
                name: "MaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatTruoc_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                column: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                column: "ThongTinNguoiDungMaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.DropIndex(
                name: "IX_PhieuDatTruoc_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "MaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.AddColumn<int>(
                name: "KhachHangMaKhachHang",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaKhachHang",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatTruoc_KhachHangMaKhachHang",
                table: "PhieuDatTruoc",
                column: "KhachHangMaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_KhachHang_KhachHangMaKhachHang",
                table: "PhieuDatTruoc",
                column: "KhachHangMaKhachHang",
                principalTable: "KhachHang",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
