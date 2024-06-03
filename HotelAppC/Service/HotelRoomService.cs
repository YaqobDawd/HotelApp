using DTOS;
using HotelAppC.Service.IService;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HotelAppC.Service
{
    public class HotelRoomService : IHotelRoomService
    {
        private readonly HttpClient httpClient;

        public HotelRoomService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<HotelRoomDTOS>> GetHotelRoom(string CheckInDate, string CheckOutDate)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/hotelroom?checkindate={CheckInDate}&checkoutdate={CheckOutDate}");
                var content = await response.Content.ReadAsStringAsync();
                var room = JsonConvert.DeserializeObject<IEnumerable<HotelRoomDTOS>>(content);
                return room;
            }
            catch (Exception ex)
            {
               
                throw;
            }
         
        }

        public async Task<HotelRoomDTOS> GetHotelRoomDetails(int roomId, string CheckInDate, string CheckOutDate)
        {
            var response = await httpClient
               .GetAsync($"api/hotelroom/{roomId}?checkInDate={CheckInDate}&checkOutDate={CheckOutDate}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var room = JsonConvert.DeserializeObject<HotelRoomDTOS>(content);
                return room;
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
