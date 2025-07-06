using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGiassintupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_Sach_SachMaSach",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_SachMaSach",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "SachMaSach",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaSach",
                table: "DanhGias",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaTaiKhoan",
                table: "DanhGias",
                column: "MaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_Sach_MaSach",
                table: "DanhGias",
                column: "MaSach",
                principalTable: "Sach",
                principalColumn: "MaSach",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "DanhGias",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_Sach_MaSach",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaSach",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaTaiKhoan",
                table: "DanhGias");

            migrationBuilder.AddColumn<int>(
                name: "SachMaSach",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SachMaSach",
                table: "DanhGias",
                column: "SachMaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias",
                column: "TaiKhoanNguoiDungMaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_Sach_SachMaSach",
                table: "DanhGias",
                column: "SachMaSach",
                principalTable: "Sach",
                principalColumn: "MaSach",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias",
                column: "TaiKhoanNguoiDungMaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
