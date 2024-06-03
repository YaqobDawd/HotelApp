using DataAccess.Model;
using DTOS;

namespace HotelAppC.Service.IService
{
    public interface IRoomOrderService
    {
        public Task<RoomOrderDetailDTO> SaveRoomOrder(RoomOrderDetailDTO roomOrderDetailDTO);
        public Task<RoomOrderDetailDTO> MarkPaymentSuccessfull(RoomOrderDetailDTO roomOrderDetailDTO);
        public Task<RoomOrderDetailDTO> GetRoomOrderDetails(int roomOrderId);
    }
}
