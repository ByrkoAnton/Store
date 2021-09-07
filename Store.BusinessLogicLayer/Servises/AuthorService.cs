using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(AuthorModel model)
        {
            throw new System.NullReferenceException();
            if (model is null || string.IsNullOrWhiteSpace(model.Name))//TODO AB: can simplify to one condition  
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }
                      
            var author = await _authorRepository.GetByNameAsync(model.Name);
            if (author is not null)
            {
                throw new CustomExeption(Constants.Error.AUTHOR_CREATE_FAILD, HttpStatusCode.BadRequest);
            }

            var newAuthor = _mapper.Map<Author>(model);
            await _authorRepository.CreateAsync(newAuthor);
        }
        public async Task<AuthorModel> GetByIdAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepository.GetByIdAsync(default);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_ID, HttpStatusCode.BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author);
            return authorModel;
        }
        public async Task<NavigationModelBase<AuthorModel>> GetAsync(AuthorFiltrationModel model)
        {
            var authorFiltrPagingSortModelDAL = _mapper.Map<AuthorFiltrationModelDAL>(model);

            (IEnumerable<Author> authors, int count) authorsWithCount = await _authorRepository.GetAsync(authorFiltrPagingSortModelDAL);
            if (!authorsWithCount.authors.Any())
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_WITH_CONDITIONS, HttpStatusCode.BadRequest);
            }
            var authorModels = _mapper.Map<IEnumerable<AuthorModel>>(authorsWithCount.authors);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(authorsWithCount.count, model.CurrentPage, model.PageSize);
            NavigationModelBase<AuthorModel> result = new NavigationModelBase<AuthorModel>
            {
                PageModel = paginatedPage,
                Models = authorModels
            };
            return result;
        }
        public async Task<AuthorModel> GetByNameAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepository.GetByNameAsync(model.Name);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_WITH_CONDITIONS, HttpStatusCode.BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author);

            return authorModel;
        }
        public async Task RemoveAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepository.GetByNameAsync(model.Name);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.AUTHOR_REMOVE_FAILD, HttpStatusCode.BadRequest);
            }
            await _authorRepository.RemoveAsync(author);
        }
        public async Task UpdateAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepository.GetByIdAsync(model.Id);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_WITH_CONDITIONS, HttpStatusCode.BadRequest);
            }

            author = _mapper.Map<Author>(model);

            await _authorRepository.UpdateAsync(author);
        }
    }
}
