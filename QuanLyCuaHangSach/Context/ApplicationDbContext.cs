using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Models;

namespace QuanLyCuaHangSach.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TheLoai> TheLoai { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<Sach> Sach { get; set; }
        public DbSet<KhoSach> KhoSach { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
  
        public DbSet<TaiKhoanNguoiDung> TaiKhoanNguoiDung { get; set; }
        public DbSet<ThongTinNguoiDung> ThongTinNguoiDung { get; set; }
        public DbSet<PhieuMuon> PhieuMuon { get; set; }
        public DbSet<PhieuTra> PhieuTra { get; set; }
        public DbSet<PhieuDatTruoc> PhieuDatTruoc { get; set; }
        public DbSet<HoaDonBan> HoaDonBan { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public DbSet<ChiTietPhieuDatTruoc> ChiTietPhieuDatTruoc { get; set; }

        public DbSet<GioHang> GioHang { get; set; } // Đảm bảo có dòng này
        public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; } // Đảm bảo có dòng này

        public DbSet<HoaDonBanOnline> HoaDonBanOnline { get; set; }
        public DbSet<ChiTietHoaDonBanOnline> ChiTietHoaDonBanOnline { get; set; }

  




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChiTietHoaDon>().HasKey(ct => new { ct.MaHoaDon, ct.MaSach });
            modelBuilder.Entity<HoaDonBan>().HasKey(hd => hd.MaHoaDon);
            modelBuilder.Entity<KhachHang>().HasKey(kh => kh.MaKhachHang);
            modelBuilder.Entity<KhoSach>().HasKey(ks => new { ks.MaKho, ks.MaSach });
            modelBuilder.Entity<NhaCungCap>().HasKey(ncc => ncc.MaNhaCungCap);
           
            modelBuilder.Entity<PhieuDatTruoc>().HasKey(pdt => pdt.MaPhieuDatTruoc);
            modelBuilder.Entity<PhieuMuon>().HasKey(pm => pm.MaPhieuMuon);
            modelBuilder.Entity<PhieuTra>().HasKey(pt => pt.MaPhieuTra);
            modelBuilder.Entity<Sach>().HasKey(s => s.MaSach);
            modelBuilder.Entity<TaiKhoanNguoiDung>().HasKey(tk => tk.MaTaiKhoan);
            modelBuilder.Entity<TheLoai>().HasKey(tl => tl.MaTheLoai); // Sửa lại khai báo khóa chính
            modelBuilder.Entity<ThongTinNguoiDung>().HasKey(tt => tt.MaThongTinNguoiDung);


            // Thiết lập quan hệ
            modelBuilder.Entity<Sach>()
                .HasOne(s => s.TheLoai)
                .WithMany(tl => tl.Sach)
                .HasForeignKey(s => s.MaTheLoai);

            modelBuilder.Entity<Sach>()
                .HasOne(s => s.NhaCungCap)
                .WithMany(ncc => ncc.Sach)
                .HasForeignKey(s => s.MaNhaCungCap);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.KhachHang)
                .WithMany(kh => kh.PhieuMuon)
                .HasForeignKey(pm => pm.MaKhachHang);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(pt => pt.PhieuMuon)
                .WithOne()
                .HasForeignKey<PhieuTra>(pt => pt.MaPhieuMuon);

            modelBuilder.Entity<ChiTietHoaDon>()
                .HasOne(ct => ct.HoaDon)
                .WithMany(hd => hd.ChiTietHoaDon)
                .HasForeignKey(ct => ct.MaHoaDon);

            modelBuilder.Entity<ChiTietHoaDon>()
                .HasOne(ct => ct.Sach)
                .WithMany()
                .HasForeignKey(ct => ct.MaSach);

            modelBuilder.Entity<TaiKhoanNguoiDung>()
                .HasOne(t => t.ThongTinNguoiDung)
                .WithOne(tn => tn.TaiKhoanNguoiDung)
                .HasForeignKey<ThongTinNguoiDung>(tn => tn.MaTaiKhoan);
            // Thiết lập quan hệ giữa PhieuDatTruoc và TaiKhoanNguoiDung

            modelBuilder.Entity<PhieuDatTruoc>()
        .HasOne(p => p.TaiKhoanNguoiDung)
        .WithMany(tk => tk.PhieuDatTruoc)
        .HasForeignKey(p => p.MaTaiKhoan)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietHoaDonBanOnline>()
        .HasOne(c => c.HoaDonBanOnline)
        .WithMany(h => h.ChiTietHoaDonBanOnline)
        .HasForeignKey(c => c.MaHoaDonOnline);

        }



    }
}
