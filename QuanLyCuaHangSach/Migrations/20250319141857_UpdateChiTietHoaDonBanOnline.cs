using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChiTietHoaDonBanOnline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoaDonBanOnline",
                columns: table => new
                {
                    MaHoaDonOnline = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonBanOnline", x => x.MaHoaDonOnline);
                    table.ForeignKey(
                        name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonBanOnline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDonOnline = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDonBanOnline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonBanOnline_HoaDonBanOnline_MaHoaDonOnline",
                        column: x => x.MaHoaDonOnline,
                        principalTable: "HoaDonBanOnline",
                        principalColumn: "MaHoaDonOnline",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonBanOnline_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaHoaDonOnline",
                table: "ChiTietHoaDonBanOnline",
                column: "MaHoaDonOnline");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaSach",
                table: "ChiTietHoaDonBanOnline",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDonBanOnline");

            migrationBuilder.DropTable(
                name: "HoaDonBanOnline");
        }
    }
}
