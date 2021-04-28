using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {

        private readonly IPaymentServise _paymentServise;

        public PaymentController(IPaymentServise paymentServise)
        {
            _paymentServise = paymentServise;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentCreationModel model)
        {
             await _paymentServise.CreateAsync(model);
             return Ok();

        }
    }
}
