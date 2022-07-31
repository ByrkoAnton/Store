using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderServise;//TODO spelling

        public OrderController(IOrderService orderServise)//TODO spelling
        {
            _orderServise = orderServise;
        }
        //TODO extra line

        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] OrderModel model)
        {
            var result = await _orderServise.GetByIdAsync(model.Id);
            return Ok(result);
        }
 
        [HttpPost("Get")]
        public async Task<IActionResult> Get(OrderFiltrationModel model)
        {
            var result = await _orderServise.GetAsync(model);
            return Ok(result);
        }
    }
}
