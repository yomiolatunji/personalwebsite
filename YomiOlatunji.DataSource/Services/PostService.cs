using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.DataSource.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostManager _postManager;

        public PostService(ApplicationDbContext context, IPostManager postManager)
        {
            _context = context;
            _postManager = postManager;
        }
        public async Task<PagedList<Post>> GetPost(int intPageIndex = 1, int intPageSize = 20, Expression<Func<Post, bool>>? filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<Post> query = _context.Posts;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Where(a => !a.IsDeleted);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                             (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return PagedList<Post>.Create(query, intPageIndex, intPageSize);
        }

        public async Task<PageModel<Post>> GetAllPost(int intPageIndex, int intPageSize)
        {
            IQueryable<Post> query = _context.Posts;
            decimal totalCount = await query.CountAsync();

            int pageCount = (int)Math.Ceiling(totalCount / intPageSize);
            var list = await query.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToListAsync();

            return new PageModel<Post> { DataCount = (int)totalCount, PageCount = pageCount, PageNumber = intPageIndex, PageSize = intPageSize, Data = list };
        }

        public async Task<IList<Post>> GetAllPost()
        {
            return await _context.Posts.Where(a => !a.IsDeleted).ToListAsync();
        }

        public async Task<bool> AddPost(Post post, string userName)
        {
            post.CanComment = true;
            post.IsDeleted = false;
            post.IsPublished = false;
            post.IsArchived = false;
            post.CreateTime = DateTimeOffset.Now;
            post.CreateBy = userName;
            post.Content = await _postManager.ExtractImageFromPost(post.Content);
            var slug = _postManager.ExtractUrlFromTitle(post.Title);
            if (_context.Posts.Any(a => a.Slug.ToLower()==slug.ToLower()))
            {
                slug = $"{slug}_{DateTime.Now:yyMMddHHmm}";
            }
            post.Slug = slug;
            post.Excerpt = _postManager.ExtractExcerptFromPost(post.Content);

            post.HeaderImage = Regex.Match(post.Content, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1]
                .Value;
            post.HeaderImage=post.HeaderImage.Replace("\"", "/");

            _context.Add(post);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> PublishPost(long postId, string userName)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(a => a.Id == postId);
            if (post == null)
            {
                return false;
            }
            post.PublishDate = DateTimeOffset.Now;
            post.UpdateBy = userName;
            post.UpdateTime = DateTimeOffset.Now;
            post.IsPublished = true;

            //_repository.Update(category);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Post> GetPostBySlug(string slug)
        {
            var post = await _context.Posts.Include(s => s.Category).Include(a => a.Tags).Include(a => a.Comments).FirstOrDefaultAsync(a => a.Slug.ToLower() == slug.ToLower());
            return post;
        }

        public async Task<bool> AddComment(AddComment comment)
        {
            Comment com = new Comment
            {
                AuthorEmail = comment.Email,
                AuthorizeTime = DateTime.Now,
                AuthorName = comment.Name,
                Message = comment.Message,
                CreateTime = DateTime.Now,
                IsAuthorized = true,
                PostId = comment.PostId
            };
            _context.Add(com);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Post> GetPostById(long id)
        {
            return await _context.Posts.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
