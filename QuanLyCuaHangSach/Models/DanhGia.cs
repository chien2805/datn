using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    public class DanhGia
    {
        public int Id { get; set; }

        public int MaSach { get; set; }  // Khóa ngoại đến bảng Sach

        [ForeignKey("MaSach")]
        public Sach? Sach { get; set; }  // Navigation property

        public int MaTaiKhoan { get; set; }

        [ForeignKey("MaTaiKhoan")]
        public TaiKhoanNguoiDung? TaiKhoanNguoiDung { get; set; }

        public int SoSao { get; set; }

        public string BinhLuan { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }
    }
}
