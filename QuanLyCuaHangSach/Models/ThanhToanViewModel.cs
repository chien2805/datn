namespace QuanLyCuaHangSach.Models
{
    public class ThanhToanViewModel
    {
        public string FullName { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public decimal TongTien { get; set; }

        // List này sẽ được model‐binder gán đầy đủ
        public List<ChiTietDatHangModel> DanhSachSach { get; set; } = new();
    }

}
