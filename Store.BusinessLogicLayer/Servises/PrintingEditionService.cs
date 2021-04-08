using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
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

            foreach (var i in model.AuthorModels)
            {
                var authors = await _authorRepository.GetAsync(authors => authors.Name == i.Name);
                if (!authors.Any())
                {
                    throw new CustomExeption(Constants.Constants.Error.NO_AUTHOR_ID_IN_DB_ADD_AUTHOR_FIRST,
                    StatusCodes.Status400BadRequest);
                }
            }

            var printingEdition = _mapper.Map<PrintingEdition>(model);
            await _printingEditionRepository.CreateAsync(printingEdition);
        }
    }
}
