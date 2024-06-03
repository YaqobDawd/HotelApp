using Business.IRepo;
using DataAccess.Model;
using DTOS;
using DTOS.PaymentInfo;
using HotelAppC.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace HotelAppC.Service
{
    public class RoomOrderService : IRoomOrderService
    {
        private readonly HttpClient httpClient;

        public RoomOrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<RoomOrderDetailDTO> MarkPaymentSuccessfull(RoomOrderDetailDTO roomOrderDetailDTO)
        {
            var content=JsonConvert.SerializeObject(roomOrderDetailDTO);
            var bodyContent=new StringContent(content,Encoding.UTF8,"application/json");
            var response = await httpClient.PostAsync("/api/roomorder/PaymentSuccessful", bodyContent); ;
            if(response.IsSuccessStatusCode)
            {
                var contentTemp=await response.Content.ReadAsStringAsync();
                var result=JsonConvert.DeserializeObject<RoomOrderDetailDTO>(contentTemp);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var error=JsonConvert.DeserializeObject<ErrorDTO>(contentTemp);
                throw new Exception(error.ErrorMessage);
            }

        }

        public async Task<RoomOrderDetailDTO> SaveRoomOrder(RoomOrderDetailDTO detailDTO)
        {
            detailDTO.hotelRoomDTO.ImageUrls= new List<string>();
            var content = JsonConvert.SerializeObject(detailDTO);
            var bodycontent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/roomorder/create", bodycontent);
            string res =  response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var ContentTemp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RoomOrderDetailDTO>(ContentTemp);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorDTO>(contentTemp);
                throw new Exception(error.ErrorMessage);

            }
        }

        public async Task<RoomOrderDetailDTO> GetRoomOrderDetails(int roomOrderId)
        {
            var response = await httpClient
                .GetAsync($"api/roomorder/GetRoomOrderDetail/{roomOrderId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var roomOrderDetails = JsonConvert.DeserializeObject<RoomOrderDetailDTO>(content);
                return roomOrderDetails;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorDTO>(content);
                throw new Exception(error.ErrorMessage);
            }
        }

    }
}
