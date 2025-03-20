using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddPhuongThucThanhToanToHoaDonBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDonOnlineChiTiet");

            migrationBuilder.DropTable(
                name: "HoaDonOnline");

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDonBan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "HoaDonBan");

            migrationBuilder.CreateTable(
                name: "HoaDonOnline",
                columns: table => new
                {
                    MaHoaDonOnline = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonOnline", x => x.MaHoaDonOnline);
                    table.ForeignKey(
                        name: "FK_HoaDonOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonOnlineChiTiet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDonOnline = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonOnlineChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoaDonOnlineChiTiet_HoaDonOnline_MaHoaDonOnline",
                        column: x => x.MaHoaDonOnline,
                        principalTable: "HoaDonOnline",
                        principalColumn: "MaHoaDonOnline",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonOnlineChiTiet_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonOnline_MaTaiKhoan",
                table: "HoaDonOnline",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonOnlineChiTiet_MaHoaDonOnline",
                table: "HoaDonOnlineChiTiet",
                column: "MaHoaDonOnline");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonOnlineChiTiet_MaSach",
                table: "HoaDonOnlineChiTiet",
                column: "MaSach");
        }
    }
}
