using Microsoft.AspNetCore.Mvc;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorServiceController : Controller
    {
        private readonly AuthorService _authorService;

        public AuthorServiceController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Author authorModel)
        {
            await _authorService.CreateAsync(authorModel);
            return Ok();
        }
    }
}
