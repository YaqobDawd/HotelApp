using DTOS.PaymentInfo;

namespace HotelAppC.Service.IService
{
    public interface IStripePaymentService
    {
        public Task <SuccessDTO> CheckOut(StripePaymentDTO stripePaymentDTO);
    }
}
