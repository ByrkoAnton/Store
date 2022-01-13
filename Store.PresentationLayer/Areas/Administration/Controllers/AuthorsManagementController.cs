using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Repositories.Interfaces;

using System.Threading.Tasks;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AuthorsManagementController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsManagementController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAuthors(string sortBy = "Name", bool isAsc = true, int page = 1)
        {
            var sortModel = new AuthorFiltrationModel
            {
                Name = HttpContext.Request.Cookies["authorNameForSearch"],
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                CurrentPage = page,
            };
            var result = await _authorService.GetAsync(sortModel);

            return View("Authors", result);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAuthors(AuthorFiltrationModel model)
        {

            var result = await _authorService.GetAsync(model);
            return View("Users", result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAuthorProfile(long id)
        {
            var result = await _authorService.GetByIdAsync(id);

            return View("AuthorProfile", result);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAuthor(AuthorModel updateModel)
        {
            var result = await _authorService.GetByIdAsync(updateModel.Id);
            result.Name = updateModel.Name;
            await _authorService.UpdateAsync(result);
            var qwery = HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(qwery); 
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddNewAuthor()
        {
            return View();

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewAuthor(AuthorModel model)
        {
            await _authorService.CreateAsync(model);
            return View("AddNewAuthor");
        }
    }
}
