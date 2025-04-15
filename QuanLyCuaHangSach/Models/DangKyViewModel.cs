using System.ComponentModel.DataAnnotations;

public class DangKyViewModel
{
    [Required]
    [EmailAddress]
    public string TenDangNhap { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string XacNhanMatKhau { get; set; }
}
