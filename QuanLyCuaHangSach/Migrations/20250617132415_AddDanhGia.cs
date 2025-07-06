using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SachMaSach = table.Column<int>(type: "int", nullable: false),
                    TaiKhoanNguoiDungMaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGias_Sach_SachMaSach",
                        column: x => x.SachMaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGias_TaiKhoanNguoiDung_TaiKhoanNguoiDungMaTaiKhoan",
                        column: x => x.TaiKhoanNguoiDungMaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SachMaSach",
                table: "DanhGias",
                column: "SachMaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_TaiKhoanNguoiDungMaTaiKhoan",
                table: "DanhGias",
                column: "TaiKhoanNguoiDungMaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGias");
        }
    }
}
