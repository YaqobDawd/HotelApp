using AutoMapper;
using Business.IRepo;
using Common;
using DataAccess;
using DataAccess.Model;
using DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repo
{
    public class RoomOrderRepo : IRoomOrderRepo
    {
        private readonly ApplicationDBContext db;
        private readonly IMapper mapper;

        public RoomOrderRepo(ApplicationDBContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<RoomOrderDetailDTO> CreateOrder(RoomOrderDetailDTO detailDTO)
        {
            try
            {
                detailDTO.CheckInDate = detailDTO.CheckInDate.Date;
                detailDTO.CheckOutDate = detailDTO.CheckOutDate.Date;
                var roomOrder=mapper.Map<RoomOrderDetailDTO,RoomOrderDetail>(detailDTO);
                roomOrder.Status = SD.Pending;
                var result=await db.RoomOrderDetail.AddAsync(roomOrder); 
                await db.SaveChangesAsync();
                return mapper.Map<RoomOrderDetail,RoomOrderDetailDTO>(result.Entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<RoomOrderDetailDTO>> GetAllOrder()
        {
            try
            {
                IEnumerable<RoomOrderDetailDTO> roomOrders=
                    mapper.Map<IEnumerable<RoomOrderDetail>,IEnumerable< RoomOrderDetailDTO >>
                    (db.RoomOrderDetail.Include(rod => rod.HotelRoom));

                return roomOrders;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<RoomOrderDetailDTO> GetOrderDetail(int OrderId)
        {
            RoomOrderDetail? roomOrder = await db.RoomOrderDetail.Include(rod => rod.HotelRoom).ThenInclude(hr => hr.RoomImages)
                 .FirstOrDefaultAsync(rod => rod.Id == OrderId);

            RoomOrderDetailDTO roomOrderDetailsDTO = mapper.Map<RoomOrderDetail, RoomOrderDetailDTO>(roomOrder);
            roomOrderDetailsDTO.hotelRoomDTO.TotalNights = roomOrderDetailsDTO.CheckOutDate
                .Subtract(roomOrderDetailsDTO.CheckInDate).Days;

            return roomOrderDetailsDTO;
        }

        public async Task<RoomOrderDetailDTO> MarkPaymentSuccessfull(int Id)
        {
            var order = await db.RoomOrderDetail.FindAsync(Id);
            if(order == null) { return null; }
            if (!order.IsPaymentSuccessful)
            {
                order.IsPaymentSuccessful=true;
                order.Status = SD.Booked;
                var markpaymentSuccessfull = db.RoomOrderDetail.Update(order);
                await db.SaveChangesAsync();
                return mapper.Map<RoomOrderDetail,RoomOrderDetailDTO>(markpaymentSuccessfull.Entity);

            }
            return new RoomOrderDetailDTO();
        }

        public async Task<bool> UpdateOrderStatus(int OrderId, string status)
        {
            try
            {
                var roomOrder = await db.RoomOrderDetail.FirstOrDefaultAsync(rod => rod.Id == OrderId);
                if (roomOrder == null)
                {
                    return false;
                }
                roomOrder.Status = status;
                if (status == SD.CheckedIn)
                {
                    roomOrder.ActualCheckInDate = DateTime.Now;
                }
                if (status == SD.CheckedOut_Completed)
                {
                    roomOrder.ActualCheckOutDate = DateTime.Now;
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
