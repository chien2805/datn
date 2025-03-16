namespace QuanLyCuaHangSach.Models
{
    // Model Phiếu trả sách
    public class PhieuTra
    {
        public int MaPhieuTra { get; set; }
        public int MaPhieuMuon { get; set; }
        public PhieuMuon PhieuMuon { get; set; }
        public DateTime NgayTra { get; set; }
    }
}
