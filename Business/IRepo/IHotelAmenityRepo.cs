using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepo
{
    public interface IHotelAmenityRepo
    {
        public  Task<HotelAmenityDTO> CreateHotelAmenity(HotelAmenityDTO hotelAmenityDTO);
        public Task<HotelAmenityDTO> UpdateHotelAmenity(HotelAmenityDTO hotelAmenityDTO, int amenityId);
        public Task<IEnumerable<HotelAmenityDTO>> GetAllHotelAmenity();
        public Task<HotelAmenityDTO> GetHotelAmenity(int amenityId);
        public Task<bool> IsAmenityUnique(string RoomName, int roomId);
        public Task<int> DeleteHotelAmenity(int roomId);

    }
}
