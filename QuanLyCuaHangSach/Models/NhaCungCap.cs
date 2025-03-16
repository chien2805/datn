namespace QuanLyCuaHangSach.Models
{
    // Model Nhà cung cấp
    public class NhaCungCap
    {
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string ThongTinLienHe { get; set; }
        public List<Sach> Sach { get; set; } = new List<Sach>();
    }
}
