namespace QuanLyCuaHangSach.Models
{
    // Model Hóa đơn bán sách
    public class HoaDonBan
    {
        public int MaHoaDon { get; set; } // Đây là khóa chính
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string TenNhanVien { get; set; }
        public List<ChiTietHoaDon> ChiTietHoaDon { get; set; } = new List<ChiTietHoaDon>();
    }

}
