namespace QuanLyCuaHangSach.Models
{
    // Model Phiếu mượn sách
    public class PhieuMuon
    {
        public int MaPhieuMuon { get; set; }
        public int MaKhachHang { get; set; }
        public KhachHang KhachHang { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime HanTra { get; set; }
    }
}
