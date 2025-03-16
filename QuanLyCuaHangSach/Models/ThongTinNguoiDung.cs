using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    // Model Thông tin người dùng
    public class ThongTinNguoiDung
    {
        public int MaThongTinNguoiDung { get; set; } // Khóa chính
        public string? HoTen { get; set; }
        public string? Email { get; set; }

        
        public string? AnhDaiDien { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }

        // Khóa ngoại + là cũng khóa chính để tạo quan hệ 1-1
        [ForeignKey("TaiKhoanNguoiDung")]
        public int MaTaiKhoan { get; set; }

        [BindNever] // Bỏ qua khi binding, không cần trong form
        public TaiKhoanNguoiDung? TaiKhoanNguoiDung { get; set; }

    }
}
