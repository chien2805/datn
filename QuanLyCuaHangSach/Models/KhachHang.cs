namespace QuanLyCuaHangSach.Models
{
    // Model Khách hàng
    public class KhachHang
    {
        public int MaKhachHang { get; set; } // Khóa chính
        public int MaThongTinNguoiDung { get; set; }
        public ThongTinNguoiDung ThongTinNguoiDung { get; set; }
        public List<PhieuMuon> PhieuMuon { get; set; } = new List<PhieuMuon>();

    }

}
