using System;
using System.Collections.Generic;

namespace QuanLyCuaHangSach.Models
{
    public class ThongKeViewModel
    {
        // Tổng tiền từ các loại
        public decimal TongTienBanOnline { get; set; }
        public decimal TongTienBanQuay { get; set; }
        public decimal TongTienMuonSach { get; set; }
        public decimal TongTatCa { get; set; }

        // Số lượng sách từ các loại
        public int SoLuongBanOnline { get; set; }
        public int SoLuongBanQuay { get; set; }
        public int SoLuongMuonSach { get; set; }
        public int SoLuongTong { get; set; }

        public List<ThongKeTheoNgay> BanOnlineTheoNgay { get; set; }
        public List<ThongKeTheoNgay> BanQuayTheoNgay { get; set; }
        public List<ThongKeTheoNgay> MuonSachTheoNgay { get; set; }
    }
    public class ThongKeTheoNgay
    {
        public DateTime Ngay { get; set; }
        public decimal TongTien { get; set; }
        public int SoLuong { get; set; }
    }
}
