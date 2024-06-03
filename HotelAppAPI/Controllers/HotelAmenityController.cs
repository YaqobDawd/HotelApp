using Business.IRepo;
using DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAmenityController : ControllerBase
    {
        private readonly IHotelAmenityRepo hotelAmenity;

        public HotelAmenityController(IHotelAmenityRepo hotelAmenity )
        {
            this.hotelAmenity = hotelAmenity;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelAmenity()
        {
            var allAmenity=await hotelAmenity.GetAllHotelAmenity();
            return Ok(allAmenity);
        }

        [HttpGet("{AmenityId}")]
        public async Task<IActionResult> GetHotelAmenity(int? AmenityId)
        {
            if(AmenityId==null|| AmenityId == 0)
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Amenity ID"
                });
                
            }
            var amenityDetail = await hotelAmenity.GetHotelAmenity(AmenityId.Value);
            return Ok(amenityDetail);
        }
    }
}
