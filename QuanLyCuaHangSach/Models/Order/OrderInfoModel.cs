namespace QuanLyCuaHangSach.Models.Order
{
    public class OrderInfoModel
    {
        public string FullName { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public double Amount { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        // Danh sách sách trong giỏ hàng
        public List<ChiTietHoaDonOnline> DanhSachSach { get; set; }
    }
}
