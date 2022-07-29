using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Stripe;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {

        private readonly IPaymentService _paymentServise;//TODO spelling

        public PaymentController(IPaymentService paymentServise)//TODO spelling
        {
            _paymentServise = paymentServise;
        }

        [HttpPost("Pay")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Pay([FromBody] StripePayModel model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var result = await _paymentServise.PayAsync(model, accessToken);
            return Ok(result);
        }
    }
}
