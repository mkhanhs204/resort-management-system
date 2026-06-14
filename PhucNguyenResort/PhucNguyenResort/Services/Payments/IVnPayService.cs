using PhucNguyenResort.ViewModels.Payments;

namespace PhucNguyenResort.Services.Payments
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VNPayRequestModel model);
        VNPayResponseModel ExecutePayment(IQueryCollection collections);
    }
}
