using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorRepository _authorRepository;

        private readonly IMapper _mapper;
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IMapper mapper,
            IAuthorRepository authorRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public async Task<PrintingEditionModel> GetByIdAsync(long id)
        {
            if (id == Constants.Variables.WRONG_ID)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                                    StatusCodes.Status400BadRequest);
            }
            var edition = await _printingEditionRepository.GetByIdAsync(id);
            if (edition is null)
            {
                throw new CustomExeption(Constants.Error.NO_EDITION_ID_IN_DB,
                   StatusCodes.Status400BadRequest);
            }
            var editionModel = _mapper.Map<PrintingEditionModel>(edition);
            return editionModel;
        }
        public async Task CreateAsync(PrintingEditionModel model)
        {
            if (!model.AuthorModels.Any())
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR,
                    StatusCodes.Status400BadRequest);
            }


            if (model.Title is null)
            {
                throw new CustomExeption(Constants.Error.NO_TITLE,
                    StatusCodes.Status400BadRequest);
            }

            var editions = await _printingEditionRepository.GetByTitle(model.Title);

            if (editions.Any())
            {
                throw new CustomExeption(Constants.Error.EDITION_EXISTS_DB,
                    StatusCodes.Status400BadRequest);
            }

            if (!_authorRepository.IsAuthorsInDb(model.AuthorModels.Select(a => a.Id).ToList()))
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_ID_IN_DB_ADD_AUTHOR_FIRST,
                StatusCodes.Status400BadRequest);
            }

            var printingEdition = _mapper.Map<PrintingEdition>(model);
            await _printingEditionRepository.CreateAsync(printingEdition);
        }
        public async Task RemoveAsync(PrintingEditionModel model)
        {
            var edition = await _printingEditionRepository.GetByTitle(model.Title);
            if (!edition.Any())
            {
                throw new CustomExeption(Constants.Error.EDITION_BY_ID_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }
            await _printingEditionRepository.RemoveAsync(edition.First());
        }
        public async Task<NavigationModel<PrintingEditionModel>> GetAsync(EditionFiltrationModel model)
        {
            if (model.MinPrise > model.MaxPrise)
            {
                model.MaxPrise = null;
                model.MinPrise = null;
            }
            var editionFiltrPagingSortModelDAL = _mapper.Map<EditionFiltrationModelDAL>(model);

            (IEnumerable<PrintingEdition> editions, int count) editionsCount = await _printingEditionRepository.GetAsync(editionFiltrPagingSortModelDAL);

            if (!editionsCount.editions.Any()) 
            {
                throw new CustomExeption(Constants.Error.NO_ANY_EDITIONS_IN_DB_WITH_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }

            var editionModels = _mapper.Map<IEnumerable<PrintingEditionModel>> (editionsCount.editions);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(editionsCount.count, model.CurrentPage, model.PageSize);
            NavigationModel<PrintingEditionModel> result = new NavigationModel<PrintingEditionModel>
            {
                PageModel = paginatedPage,
                Models = editionModels
            };
            return result;
        }
        public async Task<PrintingEditionModel> GetByTitleAsync(PrintingEditionModel model)
        {
            if (model.Title is null)
            {
                throw new CustomExeption(Constants.Error.NO_TITLE,
                    StatusCodes.Status400BadRequest);
            }

            var editions = await _printingEditionRepository.GetByTitle(model.Title);
            if (!editions.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_EDITIONS_IN_DB_WITH_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }
            var editionModel = _mapper.Map<PrintingEditionModel>(editions.First());

            return editionModel;
        }
        public async Task UpdateAsync(PrintingEditionModel model)
        {
            if (model is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            var edition = await _printingEditionRepository.GetByIdAsync(model.Id);
            if (edition is null)
            {
                throw new CustomExeption(Constants.Error.NO_EDITION_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            edition = _mapper.Map<PrintingEdition>(model);

            await _printingEditionRepository.UpdateAsync(edition);
        }
    }

}
