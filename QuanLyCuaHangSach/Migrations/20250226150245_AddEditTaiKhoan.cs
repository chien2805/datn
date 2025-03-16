using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCuaHangSach.Migrations
{
    /// <inheritdoc />
    public partial class AddEditTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_TaiKhoanNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.DropColumn(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.AlterColumn<int>(
                name: "MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoanNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "MaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "MaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_TaiKhoanNguoiDung_MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung");

            migrationBuilder.AlterColumn<int>(
                name: "MaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoanNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "ThongTinNguoiDungMaThongTinNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoanNguoiDung_ThongTinNguoiDung_ThongTinNguoiDungMaThongTinNguoiDung",
                table: "TaiKhoanNguoiDung",
                column: "ThongTinNguoiDungMaThongTinNguoiDung",
                principalTable: "ThongTinNguoiDung",
                principalColumn: "MaThongTinNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
