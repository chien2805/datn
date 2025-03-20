using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyCuaHangSach.Models
{
    public class ChiTietHoaDonBanOnline
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("HoaDonBanOnline")]
        public int MaHoaDonOnline { get; set; }

        [ForeignKey("Sach")]
        public int MaSach { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public virtual HoaDonBanOnline HoaDonBanOnline { get; set; }
        public virtual Sach Sach { get; set; }
    }

}
