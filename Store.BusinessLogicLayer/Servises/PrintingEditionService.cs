using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises//TODO spelling
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IAuthorRepositoryDapper _authorRepositoryDapper;
        private readonly IPrintingEditionRepositiryDapper _printingEditionRepositoryDapper;

        private readonly IMapper _mapper;
        public PrintingEditionService(IPrintingEditionRepositiryDapper printingEditionRepositoryDapper, IMapper mapper, IAuthorRepositoryDapper authorRepositoryDapper)
        {
            _printingEditionRepositoryDapper = printingEditionRepositoryDapper;
            _mapper = mapper;
            _authorRepositoryDapper = authorRepositoryDapper;
        }

        public async Task<PrintingEditionModel> GetByIdAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                                     HttpStatusCode.BadRequest);
            }
            var edition = await _printingEditionRepositoryDapper.GetByIdAsync(id);
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

            var edition = await _printingEditionRepositoryDapper.GetByTitleAsync(model.Title);

            if (edition is not null)
            {
                throw new CustomException(Constants.Error.EDITION_EXISTS_DB,
                     HttpStatusCode.BadRequest);
            }

            var authorsIds = model.AuthorModels.Select(a => a.Id).ToList();

            var isAuthorsInDb = await _authorRepositoryDapper.IsAuthorsInDbAsync(authorsIds);

            if (!isAuthorsInDb)
            {
                throw new CustomException(Constants.Error.ADD_AUTHOR_FIRST,
                 HttpStatusCode.BadRequest);
            }

            var printingEdition = _mapper.Map<PrintingEdition>(model);
            await _printingEditionRepositoryDapper.CreateAsync(printingEdition);
        }
        public async Task RemoveAsync(long id)
        {
            var edition = await _printingEditionRepositoryDapper.GetByIdAsync(id);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.EDITION_NOT_FOUND,
                     HttpStatusCode.BadRequest);
            }
            await _printingEditionRepositoryDapper.DeleteAsync(id);
        }
        public async Task<EditionNavigationModel> GetAsync(EditionFiltrationModel model)
        {
            var editionFiltrPagingSortModelDAL = _mapper.Map<EditionFiltrationModelDAL>(model);

            (IEnumerable<PrintingEdition> editions, int count, double minPrice, double maxPrice) editionsCount = await
                _printingEditionRepositoryDapper.GetAsync(editionFiltrPagingSortModelDAL);

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
        
        public async Task UpdateAsync(PrintingEditionModel model)
        {
            if (model is null)
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                    HttpStatusCode.BadRequest);
            }

            var edition = await _printingEditionRepositoryDapper.GetByIdAsync(model.Id);
            if (edition is null)
            {
                throw new CustomException(Constants.Error.NO_EDITION_IN_DB,
                    HttpStatusCode.BadRequest);
            }

            edition = _mapper.Map<PrintingEdition>(model);

            await _printingEditionRepositoryDapper.UpdateAsync(edition);
        }
    }

}
