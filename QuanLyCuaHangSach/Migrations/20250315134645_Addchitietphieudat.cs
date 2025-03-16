using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class Addchitietphieudat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDatTruoc_MaPhieuDatTruoc",
                table: "ChiTietPhieuDatTruoc",
                column: "MaPhieuDatTruoc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDatTruoc_MaSach",
                table: "ChiTietPhieuDatTruoc",
                column: "MaSach");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietPhieuDatTruoc");
        }
    }
}
