using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _editionService;

        public PrintingEditionController(IPrintingEditionService editionService)
        {
            _editionService = editionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromBody] PrintingEditionModel model)
        {
            var result = await _editionService.GetByIdAsync(model.Id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PrintingEditionModel model)
        {
            await _editionService.CreateAsync(model);
            return Ok();
        }
    }
}
