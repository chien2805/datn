namespace QuanLyCuaHangSach.Models
{
    public class HoaDonViewModel
    {
        public int MaHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string? TenNhanVien { get; set; } // Cho phép null
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string LoaiHoaDon { get; set; } // "quay" hoặc "online"
        public string? TrangThai { get; set; }
        public string? LoaiThanhToan { get; set; }

    }



}
