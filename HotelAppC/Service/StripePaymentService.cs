using DTOS;
using DTOS.PaymentInfo;
using HotelAppC.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace HotelAppC.Service
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly HttpClient httpClient;

        public StripePaymentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<SuccessDTO> CheckOut(StripePaymentDTO stripePaymentDTO)
        {
            var content=JsonConvert.SerializeObject(stripePaymentDTO);
            var bodycontent=new StringContent(content,Encoding.UTF8,"application/json");
            var response = await httpClient.PostAsync("/api/stripepayment/create", bodycontent);
            if(response.IsSuccessStatusCode)
            {
                var ContentTemp=await response.Content.ReadAsStringAsync();
                var result=JsonConvert.DeserializeObject<SuccessDTO>(ContentTemp);
                return result;
            }
            else
            {
                var contentTemp=await response.Content.ReadAsStringAsync();
                var error=JsonConvert.DeserializeObject<ErrorDTO>(contentTemp);
                throw new Exception(error.ErrorMessage);

            }
        }
    }
}
