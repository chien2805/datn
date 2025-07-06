namespace QuanLyCuaHangSach.Models
{
    // Model Sách
    public class Sach
    {
        public int MaSach { get; set; }
        public string TieuDe { get; set; }
        public string TacGia { get; set; }
        public int MaTheLoai { get; set; }
        public TheLoai TheLoai { get; set; }
        public int MaNhaCungCap { get; set; }
        public NhaCungCap NhaCungCap { get; set; }
        public string NhaXuatBan { get; set; }
        public int NamXuatBan { get; set; }
        public int SoLuongBan { get; set; }
        public int SoLuongMuon { get; set; }
        public int SoLuongTon { get; set; } // Số lượng sách còn lại trong kho
        public int SoLuongHong { get; set; } // Số sách bị hỏng
        public int SoLuongMat { get; set; } // Số sách bị mất
        public decimal Gia { get; set; }
        public string? HinhAnh { get; set; } // Cho phép null} // Đường dẫn hoặc tên file ảnh
        public string TrangThai { get; set; } // Con Hang, Het Hang, Dang Muon

        // Cột mới: Giới thiệu về sách
        public string? GioiThieu { get; set; }

        // ✅ Quan hệ với Đánh Giá
        public ICollection<DanhGia> DanhGias { get; set; }
    }
}
