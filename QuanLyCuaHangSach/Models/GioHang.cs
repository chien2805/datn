using System.ComponentModel.DataAnnotations;

namespace QuanLyCuaHangSach.Models
{
    public class GioHang
    {
        [Key]
        public int Id { get; set; }  // Khóa chính

        public int MaTaiKhoan { get; set; } // Liên kết với tài khoản người dùng

        public List<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

        public int TongSoLuong => ChiTietGioHangs.Sum(c => c.SoLuong);

        public decimal TongTien => ChiTietGioHangs.Sum(c => c.ThanhTien);
    }


}
