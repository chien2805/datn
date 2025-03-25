using QuanLyCuaHangSach.Models.Momo;
using QuanLyCuaHangSach.Models.Order;

namespace QuanLyCuaHangSach.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
