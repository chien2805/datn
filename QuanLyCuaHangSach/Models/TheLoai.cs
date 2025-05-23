namespace QuanLyCuaHangSach.Models
{
    // Model Thể loại sách
    public class TheLoai
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }

        public string? MoTa { get; set; }
        public List<Sach> Sach { get; set; } = new List<Sach>();
    }
}
