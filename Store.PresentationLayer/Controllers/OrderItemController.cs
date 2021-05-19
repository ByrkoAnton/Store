using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.OrderItems;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemServise;

        public OrderItemController(IOrderItemService orderItemServise)
        {
            _orderItemServise = orderItemServise;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrderItemModel model)
        {
            await _orderItemServise.CreateAsync(model);
            return Ok();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromBody] OrderItemModel model)
        {
            var result = await _orderItemServise.GetById(model.Id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderItemServise.GetAll();
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] OrderItemModel model)
        {
            await _orderItemServise.UpdateAsync(model);
            return Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove([FromBody] OrderItemModel model)
        {
            await _orderItemServise.RemoveAsync(model);
            return Ok();
        }
    }
}


