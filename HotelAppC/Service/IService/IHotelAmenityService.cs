using DTOS;

namespace HotelAppC.Service.IService
{
    public interface IHotelAmenityService
    {
        public Task<IEnumerable<HotelAmenityDTO>> GetHotelAmenity();
        public Task<HotelAmenityDTO> GetAmenityDetails(int AmenityId);
    }
}
