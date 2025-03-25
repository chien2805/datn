using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    public class ThanhToanOnline
    {
        [Key]
        public string MaThanhToan { get; set; } = Guid.NewGuid().ToString(); // Mã giao dịch

        public string MaHoaDon { get; set; } // Liên kết với hóa đơn gốc

        public DateTime NgayThanhToan { get; set; } = DateTime.Now;

        public decimal SoTien { get; set; } // Số tiền thanh toán

        public string PhuongThucThanhToan { get; set; } = "Momo"; // Phương thức thanh toán
        public bool TrangThai { get; set; } = false; // Mặc định là chưa xác nhận

        [ForeignKey("MaHoaDon")]
        public HoaDonBan? HoaDonBan { get; set; }
    }
}
