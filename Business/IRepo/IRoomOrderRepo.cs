using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IRepo
{
   public interface IRoomOrderRepo
    {
        public Task<IEnumerable<RoomOrderDetailDTO>> GetAllOrder();
        public Task<RoomOrderDetailDTO> CreateOrder(RoomOrderDetailDTO detailDTO);
        public Task<RoomOrderDetailDTO> MarkPaymentSuccessfull(int Id);
        public Task<RoomOrderDetailDTO> GetOrderDetail(int OrderId);
        public Task<bool> UpdateOrderStatus(int OrderId ,string status);


    }
}
