using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    // Model Tài khoản người dùng
    public class TaiKhoanNguoiDung
    {
        public int MaTaiKhoan { get; set; } // Khóa chính
        public string TenDangNhap { get; set; } // Email
        public string MatKhau { get; set; }
        public string VaiTro { get; set; } // Admin, NhanVien, KhachHang

        // Thêm 2 trường sau để lưu token đặt lại mật khẩu và thời hạn hiệu lực token
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        // Điều này không bắt buộc vì EF có thể tự suy ra
        public ThongTinNguoiDung ThongTinNguoiDung { get; set; }

        // Thêm danh sách phiếu đặt trước
        public List<PhieuDatTruoc>? PhieuDatTruoc { get; set; }
        // 🔹 Thêm danh sách hóa đơn bán online của người dùng này
        public List<HoaDonBanOnline>? HoaDonBanOnline { get; set; }
    }

}
