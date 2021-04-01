using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorServise _authorService;

        public AuthorController(IAuthorServise authorService)
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
            await _authorService.GetByIdAsync(model.Id);
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await _authorService.GetAllAsync();
            return Ok();
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(AuthorModel model)
        {
            await _authorService.GetByNameAsync(model);
            return Ok();
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
