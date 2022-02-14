using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorRepositoryDapper _authorRepositoryDapper;

        private readonly IMapper _mapper;
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IMapper mapper,
            IAuthorRepository authorRepository, IAuthorRepositoryDapper authorRepositoryDapper)
        {
            _printingEditionRepository = printingEditionRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _authorRepositoryDapper = authorRepositoryDapper;
        }

        public async Task<PrintingEditionModel> GetByIdAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                                     HttpStatusCode.BadRequest);
            }
            var edition = await _printingEditionRepository.GetByIdAsync(id);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.NO_EDITION_IN_DB,
                    HttpStatusCode.BadRequest);
            }
            var editionModel = _mapper.Map<PrintingEditionModel>(edition);
            return editionModel;
        }
        public async Task CreateAsync(PrintingEditionModel model)
        {
            if (!model.AuthorModels.Any())
            {
                throw new CustomException(Constants.Error.NO_AUTHOR,
                     HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new CustomException(Constants.Error.NO_TITLE,
                     HttpStatusCode.BadRequest);
            }

            var edition = await _printingEditionRepository.GetByTitle(model.Title);

            if (edition is not null)
            {
                throw new CustomException(Constants.Error.EDITION_EXISTS_DB,
                     HttpStatusCode.BadRequest);
            }

            var authorsId = model.AuthorModels.Select(a => a.Id).ToList();

            var isAuthorsInDb = _authorRepository.IsAuthorsInDb(authorsId);

            if (!isAuthorsInDb)
            {
                throw new CustomException(Constants.Error.ADD_AUTHOR_FIRST,
                 HttpStatusCode.BadRequest);
            }

            var printingEdition = _mapper.Map<PrintingEdition>(model);
            await _printingEditionRepository.CreateAsync(printingEdition);
        }
        public async Task RemoveAsync(PrintingEditionModel model)
        {
            var edition = await _printingEditionRepository.GetByIdAsync(model.Id);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.EDITION_NOT_FOUND,
                     HttpStatusCode.BadRequest);
            }
            await _printingEditionRepository.RemoveAsync(edition);
        }
        public async Task<EditionNavigationModel> GetAsync(EditionFiltrationModel model)
        {
            var editionFiltrPagingSortModelDAL = _mapper.Map<EditionFiltrationModelDAL>(model);

            (IEnumerable<PrintingEdition> editions, int count, double minPrice, double maxPrice) editionsCount = await
                _printingEditionRepository.GetAsync(editionFiltrPagingSortModelDAL);


            var editionModels = _mapper.Map<IEnumerable<PrintingEditionModel>>(editionsCount.editions);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(editionsCount.count, model.CurrentPage, model.PageSize);
            EditionNavigationModel result = new EditionNavigationModel()
            {
                PageModel = paginatedPage,
                Models = editionModels,
                SliderFloor = editionsCount.minPrice,
                SliderCeil = editionsCount.maxPrice,
            };

            return result;
        }
        public async Task<PrintingEditionModel> GetByTitleAsync(PrintingEditionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                throw new CustomException(Constants.Error.NO_TITLE,
                    HttpStatusCode.BadRequest);
            }

            var edition = await _printingEditionRepository.GetByTitle(model.Title);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.WRONG_CONDITIONS_EDITION,
                   HttpStatusCode.BadRequest);
            }
            var editionModel = _mapper.Map<PrintingEditionModel>(edition);

            return editionModel;
        }
        public async Task UpdateAsync(PrintingEditionModel model)
        {
            if (model is null)
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                    HttpStatusCode.BadRequest);
            }

            var edition = await _printingEditionRepository.GetByIdAsync(model.Id);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.NO_EDITION_IN_DB,
                    HttpStatusCode.BadRequest);
            }

            edition = _mapper.Map<PrintingEdition>(model);

            await _printingEditionRepository.UpdateAsync(edition);
        }
    }

}
