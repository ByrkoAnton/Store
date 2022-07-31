using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using Store.Sharing.Constants;
//TODO extra line
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
        public async Task<IActionResult> GetAuthors(string sortBy = Constants.AreaConstants.AUTHOR_DEF_SORT_PARAMS, bool isAsc = true, int page = Constants.AreaConstants.FIRST_PAGE)
        {
            var sortModel = new AuthorFiltrationModel
            {
                Name = HttpContext.Request.Cookies[Constants.AreaConstants.AUTHOR_NAME_COOKIES],
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                CurrentPage = page,
            };
            var result = await _authorService.GetAsync(sortModel);

            return View(Constants.AreaConstants.VIEW_AUTHORS, result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAuthorProfile(long id)
        {
            var result = await _authorService.GetByIdAsync(id);

            return View(Constants.AreaConstants.VIEW_AUTHOR_PROFILE, result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAuthor(AuthorModel updateModel)
        {
            await _authorService.UpdateAsync(updateModel);
            var qwery = HttpContext.Request.Headers[Constants.AreaConstants.PATH].ToString();//TODO spelling
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
