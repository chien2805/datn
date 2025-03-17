using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyCuaHangSach.Models
{
    public class ChiTietGioHang
    {
        [Key]
        public int Id { get; set; } // Khóa chính

        public int MaGioHang { get; set; } // Khóa ngoại liên kết với GioHang
        [ForeignKey("MaGioHang")]
        public GioHang GioHang { get; set; }

        public int MaSach { get; set; } // Khóa ngoại liên kết với Sach
        [ForeignKey("MaSach")]
        public Sach Sach { get; set; } // Điều hướng đến model Sach

        public int SoLuong { get; set; } // Số lượng sách trong giỏ hàng

        public decimal Gia => Sach?.Gia ?? 0; // Lấy giá từ Sach (tránh null)

        public string TenSach => Sach?.TieuDe ?? "Không xác định"; // Lấy tên sách từ Sach (tránh null)

        public decimal ThanhTien => Gia * SoLuong; // Tổng tiền
    }




}
