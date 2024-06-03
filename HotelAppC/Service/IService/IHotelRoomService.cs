using DTOS;
using System.Globalization;

namespace HotelAppC.Service.IService
{
    public interface IHotelRoomService
    {
        public Task<IEnumerable<HotelRoomDTOS>> GetHotelRoom(string CheckInDate, string CheckOutDate);
        public Task<HotelRoomDTOS> GetHotelRoomDetails(int roomId, string CheckInDate, string CheckOutDate);
    }
}
