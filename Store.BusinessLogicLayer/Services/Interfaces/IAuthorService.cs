﻿using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Serviсes.Interfaces //TODO wrong namespace+++
{
    public interface IAuthorService
    {
        public Task CreateAsync(AuthorModel model);
        public Task<AuthorModel> GetByIdAsync(long id);
        public Task<List<AuthorModel>> GetListOfAuthorsAsync(List<string> authors);
        public Task<NavigationModelBase<AuthorModel>> GetAsync(AuthorFiltrationModel model);
        public Task<AuthorModel> GetByNameAsync(AuthorModel model);
        public Task RemoveAsync(AuthorModel model);
        public Task UpdateAsync(AuthorModel model);
        public Task<List<AuthorModel>> GetAllAsync();
    }
}