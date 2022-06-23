using AutoMapper;
using Store.BusinessLogicLayer.AdminServises.Interfeces;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.AdminServises
{
    public class PrintingEditionServiceAdmin : IPrintingEditionServiceAdmin
    {
        private readonly IPrintingEditionService _editionService;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
    
        public PrintingEditionServiceAdmin(IPrintingEditionService printingEditionService, IMapper mapper, IAuthorService authorService)
        {
            _editionService = printingEditionService;
            _authorService = authorService;
            _mapper = mapper;
        }
        public async Task CreateEditionByAdminAsync(EditionCreateViewModel model)
        {
            var authorsList = model.AuthorsNames.Replace(Constants.AreaConstants.SPACE_IN_MODEl, string.Empty).Split(Constants.AreaConstants.DELIMETR_IN_MODEL).ToList();
            var authorsModels = await _authorService.GetListOfAuthorsAsync(authorsList);

            if (authorsList.Count != authorsModels.Count)
            {
                var wrongAuthorsList = authorsList.Except(authorsModels.Select(x => x.Name)).ToList();
                string wrongAuthors = string.Join(Constants.AreaConstants.WRONG_AUTHORS_DELIMETR, wrongAuthorsList.ToArray());
                throw new CustomException($"{Constants.AreaConstants.WRONG_AUTHORS_MSG} {wrongAuthors}", HttpStatusCode.BadRequest);
            }
               
                var editionModel = _mapper.Map<PrintingEditionModel>(model);
                editionModel.AuthorModels = authorsModels;
                await _editionService.CreateAsync(editionModel);                   
        }

        public async Task UpdateEditionByAdminAsync(EditionUpdateViewModel model)
        {
            List<string> newAuthorsList = new();
            if (model.NewAuthorsNames is not null)
            {
                newAuthorsList = model.NewAuthorsNames.Replace(Constants.AreaConstants.SPACE_IN_MODEl, string.Empty).Split(Constants.AreaConstants.DELIMETR_IN_MODEL).ToList();
            }

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
        }
    }
}
