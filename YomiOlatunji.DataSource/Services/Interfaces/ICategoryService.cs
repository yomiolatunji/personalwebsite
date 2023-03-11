using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetCategories(int intPageIndex = 1, int intPageSize = 20);
        Task<IList<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<bool> AddCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
