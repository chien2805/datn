using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    public class HoaDonBanOnline
    {
        [Key] // 🔹 Đánh dấu khóa chính
        public int MaHoaDon { get; set; }

        public string TenKhachHang { get; set; }

        public string DiaChi { get; set; }

        public string SoDienThoai { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } 
        public string LoaiThanhToan { get; set; } = "Momo"; // Xác định đây là đơn hàng online

        // 🔹 Thêm khóa ngoại liên kết đến tài khoản người dùng
        public int? MaTaiKhoan { get; set; } // nullable để đơn không cần đăng nhập vẫn được tạo

        [ForeignKey("MaTaiKhoan")]
        public TaiKhoanNguoiDung? TaiKhoanNguoiDung { get; set; }
        public List<ChiTietHoaDonOnline> ChiTietHoaDon { get; set; }
    }
}
