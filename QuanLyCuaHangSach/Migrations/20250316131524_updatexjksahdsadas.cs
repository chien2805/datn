using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class updatexjksahdsadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "MaThongTinNguoiDung",
                table: "PhieuDatTruoc");

            migrationBuilder.RenameColumn(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                newName: "TaiKhoanNguoiDungMaTaiKhoan");

            migrationBuilder.RenameIndex(
                name: "IX_PhieuDatTruoc_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                newName: "IX_PhieuDatTruoc_TaiKhoanNguoiDungMaTaiKhoan");

            migrationBuilder.AddColumn<int>(
                name: "MaTaiKhoan",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTra",
                table: "PhieuDatTruoc",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatTruoc_MaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "MaTaiKhoan");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropIndex(
                name: "IX_PhieuDatTruoc_MaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "NgayTra",
                table: "PhieuDatTruoc");

            migrationBuilder.RenameColumn(
                name: "TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc",
                newName: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_PhieuDatTruoc_TaiKhoanNguoiDungMaTaiKhoan",
                table: "PhieuDatTruoc",
                newName: "IX_PhieuDatTruoc_ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.AddColumn<int>(
                name: "MaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuDatTruoc_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "PhieuDatTruoc",
                column: "ThongTinNguoiDungMaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung");
        }
    }
}
