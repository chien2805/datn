namespace QuanLyCuaHangSach.Models
{
    // Model Kho sách
    public class KhoSach
    {
        public int MaKho { get; set; } // Có thể là khóa chính
        public int MaSach { get; set; }
        public Sach Sach { get; set; }
        public int SoLuongTon { get; set; }
        public int SoLuongHong { get; set; }
        public int SoLuongMat { get; set; }
    }

}
