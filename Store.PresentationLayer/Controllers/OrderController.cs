using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderServise;

        public OrderController(IOrderService orderServise)
        {
            _orderServise = orderServise;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrderModel model)
        {
            await _orderServise.CreateAsync(model);
            return Ok();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromBody] OrderModel model)
        {
            var result = await _orderServise.GetById(model.Id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderServise.GetAll();
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] OrderModel model)
        {
            await _orderServise.UpdateAsync(model);
            return Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody] OrderModel model)
        {
            await _orderServise.RemoveAsync(model);
            return Ok();
        }
    }
}
