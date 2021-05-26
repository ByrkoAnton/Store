using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AuthorModel model)
        {
            await _authorService.CreateAsync(model);
            return Ok();
        }

        [HttpGet("FindById")]
        public async Task<IActionResult> FindById([FromBody] AuthorModel model)
        {
           var result = await _authorService.GetByIdAsync(model.Id);
            return Ok(result);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(AuthorFiltrationModel model)
        {
            var result = await _authorService.GetAsync(model);
            return Ok(result);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(AuthorModel model)
        {
            var result = await _authorService.GetByNameAsync(model);
            return Ok(result);
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(AuthorModel model)
        {
            await _authorService.RemoveAsync(model);
            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AuthorModel model)
        {
            await _authorService.UpdateAsync(model);
            return Ok();
        }
    }
}
