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
        public async Task<IActionResult> Create([FromBody] PaymentModel model)
        {
            await _paymentServise.CreateAsync(model);
            return Ok();
        }

        [HttpGet("GetByTransactionId")]
        public async Task<IActionResult> GetByTransactionId([FromBody] PaymentModel model)
        {
            var result = await _paymentServise.GetByTransactionId(model);
             return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentServise.GetAll();
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] PaymentModel model)
        {
            await _paymentServise.UpdateAsync(model);
            return Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody] PaymentModel model)
        {
            await _paymentServise.RemoveAsync(model);
            return Ok();
        }


    }
}
