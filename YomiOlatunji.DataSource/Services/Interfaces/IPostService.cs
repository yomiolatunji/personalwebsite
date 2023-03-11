using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;

namespace YomiOlatunji.DataSource.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagedList<Post>> GetPost(int intPageIndex = 1, int intPageSize = 20, Expression<Func<Post, bool>>? filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>>? orderBy = null, string includeProperties = "");
        Task<PageModel<Post>> GetAllPost(int intPageIndex, int intPageSize = 20);
        Task<IList<Post>> GetAllPost();
        Task<Post> GetPostById(long id);
        Task<bool> AddPost(Post post, string userName);
        Task<bool> PublishPost(long postId, string userName);
        Task<Post> GetPostBySlug(string slug);
        Task<bool> AddComment(AddComment comment);
    }
}
