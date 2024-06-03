using DTOS.PaymentInfo;
using DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public StripePaymentController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create(StripePaymentDTO payment)
        {
            try
            {
                var domain = configuration.GetValue<string>("HotelAppClient_URL");

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount=payment.Amount,//convert to cents
                                Currency="usd",
                                ProductData= new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = payment.ProductName
                                }
                            },
                            Quantity=1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = domain + "/success-payment?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = domain +"/"+ payment.returnUrl
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return Ok(new SuccessDTO()
                {
                    Data = session.Id
                });

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDTO()
                {
                    ErrorMessage = e.Message
                });
            }
        }
    }

}

