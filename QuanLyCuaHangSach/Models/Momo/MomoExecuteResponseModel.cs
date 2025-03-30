namespace QuanLyCuaHangSach.Models.Momo
{
    public class MomoExecuteResponseModel
    {
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string OrderInfo { get; set; }
        public List<SachModel> DanhSachSach { get; set; } // ✅ Chuyển đổi extraData thành danh sách sách
    }
    public class SachModel
    {
        public int MaSach {  get; set; }
        public string TieuDe { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
    }
}
