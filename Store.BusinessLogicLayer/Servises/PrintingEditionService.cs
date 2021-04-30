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
            var edition = await _printingEditionRepository.GetByIdAsync(edition => edition.Id == id);
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
            var editions = await _printingEditionRepository.GetAsync(edition => edition.Description == model.Description);

            if (editions.Any())
            {
                throw new CustomExeption(Constants.Error.EDITION_EXISTS_DB,
                    StatusCodes.Status400BadRequest);
            }

            if (!model.AuthorModels.Any())
            {
                throw new CustomExeption(Constants.Error.EDITION_MUST_HAVE_AUTHOR,
                    StatusCodes.Status400BadRequest);
            }

            var allAuthors = await _authorRepository.GetAllAsync();

            var autors = allAuthors.Where(a => model.AuthorModels.Exists(p => p.Id == a.Id)).ToList();

            if (model.AuthorModels.Count != autors.Count)
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_ID_IN_DB_ADD_AUTHOR_FIRST,
                StatusCodes.Status400BadRequest);
            }

            var printingEdition = _mapper.Map<PrintingEdition>(model);
            await _printingEditionRepository.CreateAsync(printingEdition);
        }
        public async Task RemoveAsync(PrintingEditionModel model)
        {
            var edition = await _printingEditionRepository.GetAsync(edition => edition.Description == model.Description);
            if (!edition.Any())
            {
                throw new CustomExeption(Constants.Error.EDITION_BY_ID_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }
            await _printingEditionRepository.RemoveAsync(edition.First());
        }
        public async Task<NavigationModel<PrintingEditionModel>> GetAsync(EditionFiltrPaginSortModel model)
        {
            var editionFiltrPagingSortModelDAL = _mapper.Map<EditionFiltrPagingSortModelDAL>(model);

            (IEnumerable<PrintingEdition> editions, int count) editionsCount = await _printingEditionRepository.GetAsync(editionFiltrPagingSortModelDAL);

            if (!editionsCount.editions.Any()) 
            {
                throw new CustomExeption(Constants.Error.NO_ANY_EDITIONS_IN_DB_WITH_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }

            var editionModels = _mapper.Map<IEnumerable<PrintingEditionModel>> (editionsCount.count);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(editionsCount.count, model.CurrentPage, model.PageSize);
            NavigationModel<PrintingEditionModel> navigation = new NavigationModel<PrintingEditionModel>
            {
                PageModel = paginatedPage,
                EntityModels = editionModels
            };
            return navigation;
        }
        public async Task<PrintingEditionModel> GetByDescriptionAsync(PrintingEditionModel model)
        {
            var editions = await _printingEditionRepository.GetAsync(pe => pe.Description == model.Description);
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
            var edition = await _printingEditionRepository.GetByIdAsync(edition => edition.Id == model.Id);
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
