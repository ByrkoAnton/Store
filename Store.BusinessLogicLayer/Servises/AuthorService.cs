using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
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
                throw new CustomExeption(Constants.Constants.Error.AUTHOR_CREATE_FAILD_AUTHOR_ALREDY_EXISTS_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var author = _mapper.Map<Author>(model);
            await _authorRepository.CreateAsync(author);
        }

        public async Task<AuthorModel> GetByIdAsync(long id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
            {
                throw new CustomExeption(Constants.Constants.Error.NO_AUTHOR_WITH_THIS_ID_DB,
                   StatusCodes.Status400BadRequest);
            }
            var authorModel = _mapper.Map<AuthorModel>(author);
            return authorModel;
        }

        public async Task<IEnumerable<AuthorModel>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            if (!authors.Any())
            {
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_AUTHOR_IN_DB,
                   StatusCodes.Status400BadRequest);
            }

            var authorModels = _mapper.Map<IEnumerable <Author>, List<AuthorModel>>(authors);
            
            return authorModels;
        }

        public async Task<AuthorModel> GetByNameAsync(AuthorModel model)
        {
            var author = await _authorRepository.GetAsync(Authors => Authors.Name == model.Name);
            if (author is null)
            {
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_AUTHOR_IN_DB_WITH_THIS_CONDITIONS,
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
                throw new CustomExeption(Constants.Constants.Error.AUTHOR_REMOVE_FAILD_NO_AUTHOR_IN_DB,
                    StatusCodes.Status400BadRequest);
            }
            await _authorRepository.RemoveAsync(authors.First());
        }

        public async Task UpdateAsync(AuthorModel model)
        {
            var authors = await _authorRepository.GetByIdAsync(model.Id);
            if (authors is null)
            {
                throw new CustomExeption(Constants.Constants.Error.NO_ANY_AUTHOR_IN_DB_WITH_THIS_CONDITIONS,
                    StatusCodes.Status400BadRequest);
            }

            var author = _mapper.Map<Author>(model);
            await _authorRepository.UpdateAsync(author);
        }
    }
}
