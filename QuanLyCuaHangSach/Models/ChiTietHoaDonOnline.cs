using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyCuaHangSach.Models
{
    public class ChiTietHoaDonOnline
    {
        [Key] // 🔹 Đánh dấu khóa chính
        public int MaChiTiet { get; set; }

        public int MaHoaDon { get; set; } // Khóa ngoại liên kết với `HoaDonBanOnline`
        public HoaDonBanOnline HoaDon { get; set; }

        public int MaSach { get; set; } // Mã sách
        public Sach Sach { get; set; }
        public string TieuDe { get; set; }
        public int SoLuong { get; set; } // Số lượng sách đã mua
        public decimal DonGia { get; set; } // Giá sách tại thời điểm mua
        public decimal ThanhTien { get; set; }
    }

}
