using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class AuthorService : IAuthorServise
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
            var authors = await _authorRepository.GetAsync(Authors => Authors.Name == model.Name);
            if (authors.Any())
            {
                throw new CustomExeption(Constants.Error.AUTHOR_CREATE_FAILD_AUTHOR_ALREDY_EXISTS_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var author = _mapper.Map<Author>(model);
            await _authorRepository.CreateAsync(author);
        }
        public async Task<AuthorModel> GetByIdAsync(long id)
        {
            var author = await _authorRepository.GetByIdAsync(Authors => Authors.Id == id);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.NO_AUTHOR_WITH_THIS_ID_DB,
                   StatusCodes.Status400BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author);
            return authorModel;
        }
        public async Task<NavigationModel<AuthorModel>> GetAsync(AuthorFiltrPagingSortModel model)
        {
            var authorFiltrPagingSortModelDAL = _mapper.Map<AuthorFiltrPagingSortModelDAL>(model);

            (IEnumerable<Author> authors, int count) authorsWithCount = await _authorRepository.GetAsync(authorFiltrPagingSortModelDAL);
            if (!authorsWithCount.authors.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_AUTHOR_IN_DB_WITH_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }
            var authorModels = _mapper.Map<IEnumerable<AuthorModel>>(authorsWithCount.authors);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(authorsWithCount.count, model.CurrentPage, model.PageSize);
            NavigationModel<AuthorModel> result = new NavigationModel<AuthorModel>
            {
                PageModel = paginatedPage,
                EntityModels = authorModels
            };
            return result;
        }
        public async Task<AuthorModel> GetByNameAsync(AuthorModel model)
        {
            var author = await _authorRepository.GetAsync(Authors => Authors.Name == model.Name);
            if (!author.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_AUTHOR_IN_DB_WITH_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author.First());

            return authorModel;
        }
        public async Task RemoveAsync(AuthorModel model)
        {
            var authors = await _authorRepository.GetAsync(Authors => Authors.Name == model.Name);
            if (!authors.Any())
            {
                throw new CustomExeption(Constants.Error.AUTHOR_REMOVE_FAILD_NO_AUTHOR_IN_DB,
                    StatusCodes.Status400BadRequest);
            }
            await _authorRepository.RemoveAsync(authors.First());
        }
        public async Task UpdateAsync(AuthorModel model)
        {
            var author = await _authorRepository.GetByIdAsync(Authors => Authors.Id == model.Id);
            if (author is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_AUTHOR_IN_DB_WITH_THIS_CONDITIONS,
                    StatusCodes.Status400BadRequest);
            }

            author = _mapper.Map<Author>(model);

            await _authorRepository.UpdateAsync(author);
        }
    }
}
