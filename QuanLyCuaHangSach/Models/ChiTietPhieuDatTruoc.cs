using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCuaHangSach.Models
{
    public class ChiTietPhieuDatTruoc
    {
        [Key] // Định nghĩa khóa chính
        public int MaChiTiet { get; set; }
        [ForeignKey("PhieuDatTruoc")]
        public int MaPhieuDatTruoc { get; set; } // Liên kết với phiếu đặt trước
        [ForeignKey("Sach")]
        public int MaSach { get; set; } // Liên kết với sách
        public int SoLuongMuon { get; set; } // Số lượng sách được mượn

        // Khóa ngoại
        public PhieuDatTruoc PhieuDatTruoc { get; set; }
        public Sach Sach { get; set; }
    }

}
