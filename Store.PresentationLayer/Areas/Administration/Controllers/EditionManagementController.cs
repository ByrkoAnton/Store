using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.AdminServises.Interfeces;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class EditionManagementController : Controller
    {

        private readonly IPrintingEditionService _editionService;
        private readonly IPrintingEditionServiceAdmin _editionServiceAdmin;
        private readonly IAuthorService _authorService;

        public EditionManagementController(IPrintingEditionService editionService, IAuthorService authorService, IMapper mapper, IPrintingEditionServiceAdmin editionServiceAdmin)
        {
            _editionService = editionService;
            _authorService = authorService;
            _editionServiceAdmin = editionServiceAdmin;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetEditions(string sortBy = Constants.AreaConstants.EDITION_DEF_SORT_PARAMS, bool isAsc = true, int page = Constants.AreaConstants.FIRST_PAGE)
        {
            var sortModel = new EditionFiltrationModel
            {
                Title = HttpContext.Request.Cookies[Constants.AreaConstants.EDITION_TITLE_COOKIES],
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                CurrentPage = page,
                EditionType = new List<PrintingEditionType>() { PrintingEditionType.Book, PrintingEditionType.Magazine, PrintingEditionType.Newspaper }
            };
            var result = await _editionService.GetAsync(sortModel);

            return View(Constants.AreaConstants.VIEW_EDITIONS, result);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetEditions(EditionFiltrationModel model)
        {
            var result = await _editionService.GetAsync(model);
            return View(Constants.AreaConstants.VIEW_EDITIONS, result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetEditionProfile(long id)
        {
            var editionModel = await _editionService.GetByIdAsync(id);
            return View(Constants.AreaConstants.VIEW_EDITION_PROFILE, editionModel);

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewEdition()
        {
            var allAuthorsModels = await _authorService.GetAllAsync();
            var createModel = new EditionCreateViewModel() { AllAuthorModels = allAuthorsModels };
            return View(createModel);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewEdition(EditionCreateViewModel model)
        {
            await _editionServiceAdmin.CreateEditionByAdminAsync(model);
            return Ok($"{model.Title} {Constants.AreaConstants.EDITION_CREATE_MSG}");  
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateEdition(long id)
        {
            var editionModel = await _editionService.GetByIdAsync(id);
            var allAuthorsModels = await _authorService.GetAllAsync();
            var updateViewModel = new EditionUpdateViewModel() { PrintingEdition = editionModel, AllAuthorModels = allAuthorsModels };
            return View(Constants.AreaConstants.VIEW_UPDATE_EDITION, updateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateEdition(EditionUpdateViewModel model)
        {
            await _editionServiceAdmin.UpdateEditionByAdminAsync(model);
            return View(Constants.AreaConstants.VIEW_EDITION_PROFILE, await _editionService.GetByIdAsync(model.PrintingEdition.Id));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteEdition(long id, string url)
        {
            await _editionService.RemoveAsync(id);
            return Redirect(url);
        }
    }
}
