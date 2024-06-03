using Business.IRepo;
using DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;


namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomOrderController : ControllerBase
    {
        private readonly IRoomOrderRepo roomOrderRepo;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public RoomOrderController(IRoomOrderRepo roomOrderRepo,IEmailSender emailSender,IConfiguration configuration)
        {
            this.roomOrderRepo = roomOrderRepo;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomOrderDetailDTO Detail)
        {
            if(ModelState.IsValid)
            {
                var result=await roomOrderRepo.CreateOrder(Detail);
                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorDTO
                {
                    ErrorMessage = "Error While Saving room Order Details"
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful(RoomOrderDetailDTO details)
        {

            var domain = configuration.GetValue<string>("HotelAppClient_URL");


            var service = new SessionService();
            var sessionDetails = service.Get(details.StripeSessionId);
            if (sessionDetails.PaymentStatus == "paid")
            {
                var result = await roomOrderRepo.MarkPaymentSuccessfull(details.Id);
                if (result == null)
                {
                    return BadRequest(new ErrorDTO()
                    {
                        ErrorMessage = "Can not mark payment as successful"
                    });
                }
                await emailSender.SendEmailAsync(details.Email, "Booking Confirmed - Hotel App",
                     $"<h3 style ='color: purple;'>Your booking has been confirmed at Hotel App with order ID:{details.Id} and you paid {details.TotalCost}$</h3>" +
                     $"<h3 style ='color: purple;'> Check your Room's Booking Invoice on this link</h3>" +
                     $"<a href='{domain}/invoice/{details.Id}'><strong>Click here to preview you invoice </strong></a>");

                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorDTO()
                {
                    ErrorMessage = "Can not mark payment as successful"
                });
            }

        }


        [HttpGet("{roomOrderId}")]
        public async Task<IActionResult> GetRoomOrderDetail(int? roomOrderId)
        {
            if (roomOrderId == null || roomOrderId == 0)
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Room Order Id"
                });
            }

            var roomOrderDetails = await roomOrderRepo.GetOrderDetail(roomOrderId.Value);
            if (roomOrderDetails == null)
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "Room Order Not Found"
                });
            }
            return Ok(roomOrderDetails);
        }
    }
}
