using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddKhachHangToPhieuDatTruoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaTaiKhoan",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "PhieuDatTruoc",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenKhachHang",
                table: "PhieuDatTruoc",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "PhieuDatTruoc");

            migrationBuilder.DropColumn(
                name: "TenKhachHang",
                table: "PhieuDatTruoc");

            migrationBuilder.AlterColumn<int>(
                name: "MaTaiKhoan",
                table: "PhieuDatTruoc",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
