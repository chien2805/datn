namespace QuanLyCuaHangSach.Models
{

public class HoaDonPrintViewModel
{
    public int MaHoaDon { get; set; }
    public string TenKhachHang { get; set; }
    public string SoDienThoai { get; set; }
    public DateTime NgayLap { get; set; }
    public List<HoaDonDetailItem> ChiTiet { get; set; }
    public decimal TongTien { get; set; }
    public string LoaiHoaDon { get; set; } // "quay" hoặc "online"
    public string TrangThai { get; set; } // chỉ có hoá đơn online
    public string LoaiThanhToan { get; set; } // chỉ có hoá đơn online
}

public class HoaDonDetailItem
{
    public string TieuDe { get; set; }
    public decimal Gia { get; set; }
    public int SoLuong { get; set; }
    public decimal ThanhTien { get; set; }
}
}
