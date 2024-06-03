using DTOS;
using HotelAppC.Service.IService;
using Newtonsoft.Json;

namespace HotelAppC.Service
{
    public class HotelAmenityService : IHotelAmenityService
    {
        private readonly HttpClient httpClient;

        public HotelAmenityService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task<HotelAmenityDTO> GetAmenityDetails(int AmenityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HotelAmenityDTO>> GetHotelAmenity()
        {
            try
            {
                var response = await httpClient.GetAsync($"api/hotelamenity");
                var content=await response.Content.ReadAsStringAsync();
                var amenity=JsonConvert.DeserializeObject<IEnumerable<HotelAmenityDTO>>(content);
                return amenity;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
