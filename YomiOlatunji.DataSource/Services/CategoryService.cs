using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Interface;
using YomiOlatunji.DataSource.Repository;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddCategory(Category category)
        {
            _repository.Add(category);
            return await _repository.SaveChanges();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            _repository.Delete(id);
            return await _repository.SaveChanges();
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            return (IList<Category>)await _repository.Get();
        }

        public Task<IList<Category>> GetCategories(int intPageIndex = 1, int intPageSize = 20)
        {

            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _repository.Update(category);
            return await _repository.SaveChanges();
        }
    }
}
