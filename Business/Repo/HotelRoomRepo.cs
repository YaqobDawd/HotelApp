using AutoMapper;
using Business.IRepo;
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
    public class HotelRoomRepo : IHotelRoomRepo
    {
        private readonly ApplicationDBContext db;
        private readonly IMapper mapper;

        public HotelRoomRepo(ApplicationDBContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<HotelRoomDTOS> CreateHotelRoom(HotelRoomDTOS hotelRoomDTO)
        {
            HotelRoom hotel = mapper.Map<HotelRoomDTOS, HotelRoom>(hotelRoomDTO);
            hotel.CreatedDate= DateTime.Now;
            hotel.CreatedBy = "";
            var addHotelRoom=await db.HotelRooms.AddAsync(hotel);
            await db.SaveChangesAsync();
            return mapper.Map<HotelRoom, HotelRoomDTOS>(addHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomToDelete = await db.HotelRooms.FindAsync(roomId);
            if (roomToDelete != null)
            {
                var allImage= await db.RoomImage.Where(ri=>ri.RoomId == roomId).ToListAsync();
                db.RemoveRange(allImage);
                db.HotelRooms.Remove(roomToDelete);
                return await db.SaveChangesAsync();//return number of deleted records 
            }
            return 0;
        }

        public async Task<IEnumerable<HotelRoomDTOS>> GetAllHotelRoom(string CheckInDate = null, string CheckOutDate = null)
        {
            try
            {
                IEnumerable<HotelRoomDTOS> roomsDetailsDTOs =
                            mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTOS>>
                            (db.HotelRooms.Include(r=>r.RoomImages));
                if (!string.IsNullOrEmpty(CheckInDate) && !string.IsNullOrEmpty(CheckOutDate))
                {
                    foreach (var roomDetailDTO in roomsDetailsDTOs)
                    {
                        roomDetailDTO.IsBooked = await IsRoomBooked(roomDetailDTO.Id, CheckInDate, CheckOutDate);
                    }
                }
                return roomsDetailsDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<HotelRoomDTOS> GetHotelRoom(int roomId, string CheckInDate = null, string CheckOutDate = null)
        {
            try
            {
                HotelRoomDTOS? roomDetailsDTO = mapper.Map<HotelRoom, HotelRoomDTOS>(
                    await db.HotelRooms.Include(r=>r.RoomImages).FirstOrDefaultAsync(r => r.Id == roomId));

                if(!string.IsNullOrEmpty(CheckInDate)&& !string.IsNullOrEmpty(CheckOutDate))
                {
                    roomDetailsDTO.IsBooked=await IsRoomBooked(roomId, CheckInDate, CheckOutDate);
                }



                return roomDetailsDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsRoomBooked(int RoomId, string checkInDatestr, string checkOutDatestr)
        {
            try
            {
                if (!string.IsNullOrEmpty(checkOutDatestr) && !string.IsNullOrEmpty(checkInDatestr))
                {
                    DateTime checkInDate = DateTime.ParseExact(checkInDatestr, "MM/dd/yyyy", null);
                    DateTime checkOutDate = DateTime.ParseExact(checkOutDatestr, "MM/dd/yyyy", null);

                    var existingBooking = await db.RoomOrderDetail.Where(rod => rod.RoomId == RoomId && rod.IsPaymentSuccessful &&
                       //check if checkin date that user wants does not fall in between any dates for room that is booked
                       ((checkInDate < rod.CheckOutDate && checkInDate.Date >= rod.CheckInDate)
                       //check if checkout date that user wants does not fall in between any dates for room that is booked
                       || (checkOutDate.Date > rod.CheckInDate.Date && checkInDate.Date <= rod.CheckInDate.Date)
                       )).FirstOrDefaultAsync();

                    if (existingBooking != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsUnique(string RoomName ,int roomId)
        {
            if (roomId == 0)
            {
                //create
                HotelRoom? hotelRoom = await db.HotelRooms.FirstOrDefaultAsync(r => r.Name.ToLower() == RoomName.ToLower());
                if (hotelRoom == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //update
                HotelRoom? hotelRoom = await db.HotelRooms.FirstOrDefaultAsync(r => r.Name.ToLower() == RoomName.ToLower() && r.Id != roomId);
                if (hotelRoom == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<HotelRoomDTOS> UpdateHotelRoom(int roomId, HotelRoomDTOS hotelRoomDTO)
        {
            try
            {
                if (roomId == hotelRoomDTO.Id)
                {
                    HotelRoom? room2Update=await db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = mapper.Map<HotelRoomDTOS, HotelRoom>(hotelRoomDTO, room2Update);
                    room.UpdatedDate = DateTime.Now;
                    room.UpdatedBy = "";
                    var apdatedroom = db.HotelRooms.Update(room);
                    await db.SaveChangesAsync();
                    return mapper.Map<HotelRoom, HotelRoomDTOS>(apdatedroom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
}
