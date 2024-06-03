using Business.IRepo;
using Common;
using DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IHotelRoomRepo hotelRoomRepo;
        private readonly IEmailSender email;

        public HotelRoomController(IHotelRoomRepo hotelRoomRepo, IEmailSender email)
        {
            this.hotelRoomRepo = hotelRoomRepo;
            this.email = email;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelRoom(string CheckInDate, string CheckOutDate)
        {
            if (string.IsNullOrEmpty(CheckInDate) || string.IsNullOrEmpty(CheckOutDate))
            {
                return BadRequest(new ErrorDTO()
                { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(CheckInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckInDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckIn date format. valid format is MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(CheckOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckOut date format. valid format is MM/dd/yyyy"
                });
            }
            var allRooms=await hotelRoomRepo.GetAllHotelRoom(CheckInDate,CheckOutDate);
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId, string CheckInDate , string CheckOutDate)
        {
            if (string.IsNullOrEmpty(CheckInDate) || string.IsNullOrEmpty(CheckOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(CheckInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckInDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckIn date format. valid format is MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(CheckOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckOut date format. valid format is MM/dd/yyyy"
                });
            }

            if (roomId == null|| roomId == 0)
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Room Id"
                });

            }
            var roomDetails = await hotelRoomRepo.GetHotelRoom(roomId.Value,CheckInDate,CheckOutDate);
            if(roomDetails == null)
            {
                return NotFound(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "Room Not Found"
                });
            }
            return Ok(roomDetails);
        }
    }
}
