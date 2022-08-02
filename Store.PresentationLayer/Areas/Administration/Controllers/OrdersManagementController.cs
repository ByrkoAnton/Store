using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using Store.Sharing.Constants;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class OrdersManagementController : Controller
    {
        private readonly IOrderService _orderService;//TODO add new line
        public OrdersManagementController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOrders(string sortBy = Constants.AreaConstants.ORDER_DEF_SORT_PARAMS, bool isAsc = true, int page = Constants.AreaConstants.FIRST_PAGE)
        {
            bool isUserId = long.TryParse(HttpContext.Request.Cookies[Constants.AreaConstants.USER_ID_COOKIES], out long userId);
            var sortModel = new OrderFiltrationModel
            {
                UserId = isUserId ? userId : null,
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                CurrentPage = page,
                Description = HttpContext.Request.Cookies[Constants.AreaConstants.EDITION_DESC_COOKIES]//TODO wrong field
            };
            var result = await _orderService.GetAsync(sortModel);

            return View(Constants.AreaConstants.VIEW_ORDERS, result);

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var result = await _orderService.GetOrderDetails(id);

            return View(Constants.AreaConstants.VIEW_ORDER_PROFILE, result);

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            await _orderService.RemoveAsync(id);
            var query = HttpContext.Request.Headers[Constants.AreaConstants.PATH].ToString();//TODO wrong spelling+++
            return Redirect(query);
        }
    }
}
