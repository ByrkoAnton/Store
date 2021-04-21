using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                throw new CustomExeption(Constants.Constants.Error.NO_EDITION_ID_IN_DB,
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
                throw new CustomExeption(Constants.Constants.Error.EDITION_EXISTS_DB,
                    StatusCodes.Status400BadRequest);
            }

            if (!model.AuthorModels.Any())
            {
                throw new CustomExeption(Constants.Constants.Error.EDITION_MUST_HAVE_AUTHOR,
                    StatusCodes.Status400BadRequest);
            } 

            var authors = _mapper.Map<IEnumerable<Author>>(model.AuthorModels).ToList();
            var allAuthors = await _authorRepository.GetAllAsync();

            int authorsCountDifference = allAuthors.Count(p => authors.Exists(p2 => p2.Name == p.Name)) - authors.Count();
            if (authorsCountDifference != 0)
            {
                throw new CustomExeption(Constants.Constants.Error.NO_AUTHOR_ID_IN_DB_ADD_AUTHOR_FIRST,
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
                throw new CustomExeption(Constants.Constants.Error.EDITION_BY_ID_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }
            await _printingEditionRepository.RemoveAsync(edition.First());
        }
        public async Task<List<PrintingEditionModel>> GetAllAsync()
        {
            var editions = await _printingEditionRepository.GetAllAsync();
            if (!editions.Any())
            {
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_EDITION_IN_DB,
                   StatusCodes.Status400BadRequest);
            }

            var editionModels = _mapper.Map<IEnumerable<PrintingEditionModel>>(editions);

            return editionModels.ToList();
        }

        public async Task<PrintingEditionModel> GetByDescriptionAsync(PrintingEditionModel model)
        {
            var editions = await _printingEditionRepository.GetAsync(pe => pe.Description == model.Description);
            if (!editions.Any())
            {
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_EDITIONS_IN_DB_WITH_THIS_CONDITIONS,
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
                throw new CustomExeption(Constants.Constants.Error.NO_EDITION_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            edition = _mapper.Map<PrintingEdition>(model);

            await _printingEditionRepository.UpdateAsync(edition);
        }
    }

}
