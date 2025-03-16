namespace QuanLyCuaHangSach.Models
{
    public class PhieuDatTruoc
    {
        public int MaPhieuDatTruoc { get; set; }

        // Liên kết với TaiKhoanNguoiDung
        public int MaTaiKhoan { get; set; } // Nếu cho phép null: int? MaTaiKhoan { get; set; }

        public DateTime NgayDat { get; set; }
        public DateTime NgayTra { get; set; }
        public string TrangThai { get; set; }

        // Ràng buộc quan hệ
        public TaiKhoanNguoiDung TaiKhoanNguoiDung { get; set; } // Nếu nullable: TaiKhoanNguoiDung? TaiKhoanNguoiDung { get; set; }

        // Liên kết danh sách chi tiết phiếu đặt trước
        public ICollection<ChiTietPhieuDatTruoc> ChiTietPhieuDatTruoc { get; set; } = new List<ChiTietPhieuDatTruoc>();
    }
}

