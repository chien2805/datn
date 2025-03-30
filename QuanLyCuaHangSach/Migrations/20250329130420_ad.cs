using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class ad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonBan",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonBan", x => x.MaHoaDon);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThongTinLienHe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoanNguoiDung",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoanNguoiDung", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "TheLoai",
                columns: table => new
                {
                    MaTheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoai", x => x.MaTheLoai);
                });

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
                name: "PhieuDatTruoc",
                columns: table => new
                {
                    MaPhieuDatTruoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDatTruoc", x => x.MaPhieuDatTruoc);
                    table.ForeignKey(
                        name: "FK_PhieuDatTruoc_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinNguoiDung",
                columns: table => new
                {
                    MaThongTinNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinNguoiDung", x => x.MaThongTinNguoiDung);
                    table.ForeignKey(
                        name: "FK_ThongTinNguoiDung_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TacGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaTheLoai = table.Column<int>(type: "int", nullable: false),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false),
                    NhaXuatBan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamXuatBan = table.Column<int>(type: "int", nullable: false),
                    SoLuongBan = table.Column<int>(type: "int", nullable: false),
                    SoLuongMuon = table.Column<int>(type: "int", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    SoLuongHong = table.Column<int>(type: "int", nullable: false),
                    SoLuongMat = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sach", x => x.MaSach);
                    table.ForeignKey(
                        name: "FK_Sach_NhaCungCap_MaNhaCungCap",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNhaCungCap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sach_TheLoai_MaTheLoai",
                        column: x => x.MaTheLoai,
                        principalTable: "TheLoai",
                        principalColumn: "MaTheLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaThongTinNguoiDung = table.Column<int>(type: "int", nullable: false),
                    ThongTinNguoiDungMaThongTinNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                        column: x => x.ThongTinNguoiDungMaThongTinNguoiDung,
                        principalTable: "ThongTinNguoiDung",
                        principalColumn: "MaThongTinNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_GioHang_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => new { x.MaHoaDon, x.MaSach });
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HoaDonBan_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDonBan",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
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

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuDatTruoc",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieuDatTruoc = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuongMuon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuDatTruoc", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuDatTruoc_PhieuDatTruoc_MaPhieuDatTruoc",
                        column: x => x.MaPhieuDatTruoc,
                        principalTable: "PhieuDatTruoc",
                        principalColumn: "MaPhieuDatTruoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuDatTruoc_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhoSach",
                columns: table => new
                {
                    MaKho = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    SoLuongHong = table.Column<int>(type: "int", nullable: false),
                    SoLuongMat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoSach", x => new { x.MaKho, x.MaSach });
                    table.ForeignKey(
                        name: "FK_KhoSach_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuMuon",
                columns: table => new
                {
                    MaPhieuMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    NgayMuon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanTra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuMuon", x => x.MaPhieuMuon);
                    table.ForeignKey(
                        name: "FK_PhieuMuon_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuTra",
                columns: table => new
                {
                    MaPhieuTra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieuMuon = table.Column<int>(type: "int", nullable: false),
                    NgayTra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuTra", x => x.MaPhieuTra);
                    table.ForeignKey(
                        name: "FK_PhieuTra_PhieuMuon_MaPhieuMuon",
                        column: x => x.MaPhieuMuon,
                        principalTable: "PhieuMuon",
                        principalColumn: "MaPhieuMuon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_MaGioHang",
                table: "ChiTietGioHang",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_MaSach",
                table: "ChiTietGioHang",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_MaSach",
                table: "ChiTietHoaDon",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaHoaDonOnline",
                table: "ChiTietHoaDonBanOnline",
                column: "MaHoaDonOnline");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonBanOnline_MaSach",
                table: "ChiTietHoaDonBanOnline",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDatTruoc_MaPhieuDatTruoc",
                table: "ChiTietPhieuDatTruoc",
                column: "MaPhieuDatTruoc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDatTruoc_MaSach",
                table: "ChiTietPhieuDatTruoc",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonBanOnline_MaTaiKhoan",
                table: "HoaDonBanOnline",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "KhachHang",
                column: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_KhoSach_MaSach",
                table: "KhoSach",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDatTruoc_MaTaiKhoan",
                table: "PhieuDatTruoc",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuon_MaKhachHang",
                table: "PhieuMuon",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuTra_MaPhieuMuon",
                table: "PhieuTra",
                column: "MaPhieuMuon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sach_MaNhaCungCap",
                table: "Sach",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_MaTheLoai",
                table: "Sach",
                column: "MaTheLoai");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinNguoiDung_MaTaiKhoan",
                table: "ThongTinNguoiDung",
                column: "MaTaiKhoan",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDonBanOnline");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuDatTruoc");

            migrationBuilder.DropTable(
                name: "KhoSach");

            migrationBuilder.DropTable(
                name: "PhieuTra");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "HoaDonBan");

            migrationBuilder.DropTable(
                name: "HoaDonBanOnline");

            migrationBuilder.DropTable(
                name: "PhieuDatTruoc");

            migrationBuilder.DropTable(
                name: "Sach");

            migrationBuilder.DropTable(
                name: "PhieuMuon");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "TheLoai");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "ThongTinNguoiDung");

            migrationBuilder.DropTable(
                name: "TaiKhoanNguoiDung");
        }
    }
}
