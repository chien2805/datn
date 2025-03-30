using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDonBanOnline");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline");

            migrationBuilder.RenameColumn(
                name: "PhuongThucThanhToan",
                table: "HoaDonBanOnline",
                newName: "LoaiThanhToan");

            migrationBuilder.RenameColumn(
                name: "NgayLap",
                table: "HoaDonBanOnline",
                newName: "NgayTao");

            migrationBuilder.RenameColumn(
                name: "DiaChiGiaoHang",
                table: "HoaDonBanOnline",
                newName: "DiaChi");

            migrationBuilder.RenameColumn(
                name: "MaHoaDonOnline",
                table: "HoaDonBanOnline",
                newName: "MaHoaDon");

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonOnline",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    HoaDonMaHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SachMaSach = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDonOnline", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonOnline_HoaDonBanOnline_HoaDonMaHoaDon",
                        column: x => x.HoaDonMaHoaDon,
                        principalTable: "HoaDonBanOnline",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonOnline_Sach_SachMaSach",
                        column: x => x.SachMaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_HoaDonMaHoaDon",
                table: "ChiTietHoaDonOnline",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonOnline_SachMaSach",
                table: "ChiTietHoaDonOnline",
                column: "SachMaSach");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDonOnline");

            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "HoaDonBanOnline",
                newName: "NgayLap");

            migrationBuilder.RenameColumn(
                name: "LoaiThanhToan",
                table: "HoaDonBanOnline",
                newName: "PhuongThucThanhToan");

            migrationBuilder.RenameColumn(
                name: "DiaChi",
                table: "HoaDonBanOnline",
                newName: "DiaChiGiaoHang");

            migrationBuilder.RenameColumn(
                name: "MaHoaDon",
                table: "HoaDonBanOnline",
                newName: "MaHoaDonOnline");

            migrationBuilder.AddColumn<int>(
                name: "MaTaiKhoan",
                table: "HoaDonBanOnline",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonBanOnline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDonOnline = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaHoaDonOnline",
                table: "ChiTietHoaDonBanOnline",
                column: "MaHoaDonOnline");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaSach",
                table: "ChiTietHoaDonBanOnline",
                column: "MaSach");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonBanOnline_TaiKhoanNguoiDung_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoanNguoiDung",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
