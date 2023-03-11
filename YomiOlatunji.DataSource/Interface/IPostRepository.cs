using System.Linq.Expressions;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.DataSource.Interface
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<PageModel<Post>> QueryPage(Expression<Func<Post, bool>> filter = null, int intPageIndex = 1, int intPageSize = 20, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, string includeProperties = "");
    }
}