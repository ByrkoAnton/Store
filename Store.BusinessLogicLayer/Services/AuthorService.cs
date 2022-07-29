﻿using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Serviсes//TODO worng spelling+++
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepositoryDapper _authorRepositoryDapper;
        private readonly IMapper _mapper;
        public AuthorService(IMapper mapper, IAuthorRepositoryDapper authorRepositoryDapper)
        {
            _authorRepositoryDapper = authorRepositoryDapper;
            _mapper = mapper;
        }
        public async Task CreateAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }
                      
            var author = await _authorRepositoryDapper.GetByNameAsync(model.Name);
            if (author is not null)
            {
                throw new CustomException(Constants.Error.AUTHOR_CREATE_FAILD, HttpStatusCode.BadRequest);
            }

            var newAuthor = _mapper.Map<Author>(model);
            await _authorRepositoryDapper.CreateAsync(newAuthor);
        }
        public async Task<AuthorModel> GetByIdAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var authorDapper = await _authorRepositoryDapper.GetByIdAsync(id);//TODO wrong spelling+++
            if (authorDapper is null)
            {
                throw new CustomException(Constants.Error.NO_AUTHOR_ID, HttpStatusCode.BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(authorDapper);
            return authorModel;
        }

        public async Task<List<AuthorModel>> GetListOfAuthorsAsync(List<string> authorsNames)
        {        
            var authors = await _authorRepositoryDapper.GetAuthorsListByNamesListAsync(authorsNames);
            var authorsModels = _mapper.Map<IEnumerable<AuthorModel>>(authors).ToList();
            return authorsModels;
        }

        public async Task<List<AuthorModel>> GetAllAsync()
        {
            var authors = await _authorRepositoryDapper.GetAllAsync();
            var authorsModels = _mapper.Map<IEnumerable<AuthorModel>>(authors);
            
            return authorsModels.ToList();
        }
        public async Task<NavigationModelBase<AuthorModel>> GetAsync(AuthorFiltrationModel model)
        {
            var authorFiltrPagingSortModelDAL = _mapper.Map<AuthorFiltrationModelDAL>(model);

            var (authors, count) = await _authorRepositoryDapper.GetAsync(authorFiltrPagingSortModelDAL);//TODO u can simplify it var (authors, count) and them use just variable+???++


            var authorModels = _mapper.Map<IEnumerable<AuthorModel>>(authors);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(count, model.CurrentPage, model.PageSize);
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
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepositoryDapper.GetByNameAsync(model.Name);
            if (author is null)
            {
                throw new CustomException(Constants.Error.NO_AUTHOR_WITH_CONDITIONS, HttpStatusCode.BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author);

            return authorModel;
        }
        public async Task RemoveAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepositoryDapper.GetByNameAsync(model.Name);
            if (author is null)
            {
                throw new CustomException(Constants.Error.AUTHOR_REMOVE_FAILD, HttpStatusCode.BadRequest);
            }
            await _authorRepositoryDapper.DeleteAsync(author.Id);
        }
        public async Task UpdateAsync(AuthorModel model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var author = await _authorRepositoryDapper.GetByIdAsync(model.Id);
            if (author is null)
            {
                throw new CustomException(Constants.Error.NO_AUTHOR_WITH_CONDITIONS, HttpStatusCode.BadRequest);
            }

            author = _mapper.Map<Author>(model);

            await _authorRepositoryDapper.UpdateAsync(author);
        }
    }
}