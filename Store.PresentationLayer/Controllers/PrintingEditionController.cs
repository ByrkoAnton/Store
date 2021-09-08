using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
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

        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] PrintingEditionModel model)
        {
            var result = await _editionService.GetByIdAsync(model.Id);
            return Ok(result);
        }

        [HttpPost("Craete")]
        public async Task<IActionResult> Create([FromBody] PrintingEditionModel model)
        {
            await _editionService.CreateAsync(model);
            return Ok();
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] PrintingEditionModel model)
        {
            await _editionService.RemoveAsync(model);
            return Ok();
        }

        [HttpPost("GetEditions")]
        public async Task<IActionResult> GetEditions(EditionFiltrationModel model)
        {
            var result = await _editionService.GetAsync(model);
            return Ok(result);
        }

        [HttpPost("GetByDescription")]
        public async Task<IActionResult> GetByTitle(PrintingEditionModel model)
        {
            var result = await _editionService.GetByTitleAsync(model);
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PrintingEditionModel model)
        {
            await _editionService.UpdateAsync(model);
            return Ok();
        }
    }
}
