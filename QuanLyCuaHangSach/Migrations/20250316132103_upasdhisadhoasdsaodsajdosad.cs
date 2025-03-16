using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class upasdhisadhoasdsaodsajdosad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropIndex(
                name: "IX_PhieuDatTruoc_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatTruoc_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "TaiKhoanNguoiDungMaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "TaiKhoanNguoiDungMaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan");
        }
    }
}
