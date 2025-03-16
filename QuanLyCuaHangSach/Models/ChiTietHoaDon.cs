namespace QuanLyCuaHangSach.Models
{
    // Model Chi tiết hóa đơn
    public class ChiTietHoaDon
    {
        public int MaHoaDon { get; set; }
        public HoaDonBan? HoaDon { get; set; }
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public Sach? Sach { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
