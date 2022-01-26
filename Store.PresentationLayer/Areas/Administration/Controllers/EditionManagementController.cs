﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class EditionManagementController : Controller
    {

        private readonly IPrintingEditionService _editionService;
        private readonly IAuthorService _authorService;

        private readonly IMapper _mapper;

        public EditionManagementController(IPrintingEditionService editionService, IAuthorService authorService, IMapper mapper)
        {
            _editionService = editionService;
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetEditions(string sortBy = Constants.AreaConstants.EDITION_DEF_SORT_PARAMS, bool isAsc = true, int page = Constants.AreaConstants.FIRST_PAGE)
        {
            var sortModel = new EditionFiltrationModel
            {
                Title = HttpContext.Request.Cookies[Constants.AreaConstants.EDITION_TITLE_COOKIES],
                AuthorName = HttpContext.Request.Cookies[Constants.AreaConstants.EDITION_AUTHOR_COOKIES],
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
            var result = await _editionService.GetByIdAsync(id);

            return View(Constants.AreaConstants.VIEW_EDITION_PROFILE, result);

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewEdition()
        {
            var allAuthors = await _authorService.GetAllAsync();
            var createModel = new EditionCreateViewModel() { AllAuthorModels = allAuthors };
            return View(createModel);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddNewEdition(EditionCreateViewModel model)
        {
            var authorsList = model.AuthorsNames.Replace(Constants.AreaConstants.SPACE_IN_MODEl, string.Empty).Split(Constants.AreaConstants.DELIMETR_IN_MODEL).ToList();
            var authorsModels = await _authorService.GetListOfAuthorsAsync(authorsList);
            if (authorsList.Count == authorsModels.Count)
            {
                var editionModel = _mapper.Map<PrintingEditionModel>(model);
                editionModel.AuthorModels = authorsModels;
                await _editionService.CreateAsync(editionModel);

                var allAuthors = await _authorService.GetAllAsync();
                var createModel = new EditionCreateViewModel() { AllAuthorModels = allAuthors };
                return View(Constants.AreaConstants.VIEW_ADD_EDITION, createModel);
            }

            var wrongAuthorsList = authorsList.Except(authorsModels.Select(x => x.Name)).ToList();
            string wrongAuthors = string.Join(Constants.AreaConstants.WRONG_AUTHORS_DELIMETR, wrongAuthorsList.ToArray());
            throw new CustomException($"{Constants.AreaConstants.WRONG_AUTHORS_MSG} {wrongAuthors}", HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateEdition(long id)
        {
            var result = await _editionService.GetByIdAsync(id);
            var allAuthors = await _authorService.GetAllAsync();
            var updateViewModel = new EditionUpdateViewModel() { PrintingEdition = result, AllAuthorModels = allAuthors };
            return View(Constants.AreaConstants.VIEW_UPDATE_EDITION, updateViewModel);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateEdition(EditionUpdateViewModel model)
        {
            List<string> newAuthorsList = new();
            //if (model.NewAuthorsNames is not null)
            //{
            //    newAuthorsList = model.NewAuthorsNames.Replace(Constants.AreaConstants.SPACE_IN_MODEl, string.Empty).Split(Constants.AreaConstants.DELIMETR_IN_MODEL).ToList();
            //}

            List<string> delAuthorsList = new();
            if (model.DeletedAuthorsNames is not null)
            {
                delAuthorsList = model.DeletedAuthorsNames.Replace(Constants.AreaConstants.SPACE_IN_MODEl, string.Empty).Split(Constants.AreaConstants.DELIMETR_IN_MODEL).ToList();
            }

            var edition = await _editionService.GetByIdAsync(model.PrintingEdition.Id);
            var authorsNames = edition.AuthorModels.Select(x => x.Name).ToList();
            newAuthorsList.RemoveAll(x => authorsNames.Contains(x));
            authorsNames.AddRange(newAuthorsList);
            authorsNames.RemoveAll(x => delAuthorsList.Contains(x));

            var authorsModels = await _authorService.GetListOfAuthorsAsync(authorsNames);

            model.PrintingEdition.AuthorModels = authorsModels;

            await _editionService.UpdateAsync(model.PrintingEdition);

            var qwery = HttpContext.Request.Headers[Constants.AreaConstants.PATH].ToString();
            return Redirect(qwery);
        }
    }
}