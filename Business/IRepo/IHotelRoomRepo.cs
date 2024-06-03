using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepo
{
    public interface IHotelRoomRepo
    {
        public Task<HotelRoomDTOS> CreateHotelRoom(HotelRoomDTOS hotelRoomDTO);
        public Task<HotelRoomDTOS> UpdateHotelRoom(int roomId,HotelRoomDTOS hotelRoomDTO);
        public Task<IEnumerable<HotelRoomDTOS>> GetAllHotelRoom(string CheckInDate = null, string CheckOutDate = null);
        public Task<HotelRoomDTOS> GetHotelRoom(int roomId,string CheckInDate=null,string CheckOutDate=null);
        public Task<bool> IsRoomBooked(int RoomId , string CheckInDate,string CheckOutDate);
        public Task<bool> IsUnique(string RoomName, int roomId);
        public Task<int> DeleteHotelRoom(int roomId);

    }
}
