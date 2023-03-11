using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Interface;

namespace YomiOlatunji.DataSource.Repository
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        internal new ApplicationDbContext context;
        internal new DbSet<Post> dbSet;

        public PostRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
            dbSet = context.Set<Post>();
        }

        public async Task<PageModel<Post>> QueryPage(Expression<Func<Post, bool>> filter = null, int intPageIndex = 1, int intPageSize = 20, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, string includeProperties = "")
        {
            IQueryable<Post> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Where(a => !a.IsDeleted);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            decimal totalCount = await query.CountAsync();

            int pageCount = (int)Math.Ceiling(totalCount / intPageSize);

            var list = await query.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToListAsync();

            return new PageModel<Post>() { DataCount = (int)totalCount, PageCount = pageCount, PageNumber = intPageIndex, PageSize = intPageSize, Data = list };
        }
    }
}