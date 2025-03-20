using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    public class HoaDonBanOnline
    {
        [Key]
        public int MaHoaDonOnline { get; set; }

        [ForeignKey("TaiKhoanNguoiDung")]
        public int MaTaiKhoan { get; set; }

        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string TrangThai { get; set; }  // Ví dụ: "Chưa thanh toán", "Đã thanh toán",...

        public virtual List<ChiTietHoaDonBanOnline> ChiTietHoaDonBanOnline { get; set; } = new List<ChiTietHoaDonBanOnline>();

        public virtual TaiKhoanNguoiDung TaiKhoanNguoiDung { get; set; }
    }
}
