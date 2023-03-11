using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Interface;

namespace YomiOlatunji.DataSource.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
