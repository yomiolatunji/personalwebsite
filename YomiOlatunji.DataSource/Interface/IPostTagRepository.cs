using System.Linq.Expressions;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.DataSource.Interface
{
    public interface IPostTagRepository
    {
        void Add(PostTag model);

        void Add(IEnumerable<PostTag> listEntity);

        void Delete(long postId, long tagId);

        void Delete(PostTag entityToDelete);

        void Update(PostTag model);

        Task<IEnumerable<PostTag>> Get(
            Expression<Func<PostTag, bool>> filter = null,
            Func<IQueryable<PostTag>, IOrderedQueryable<PostTag>> orderBy = null,
            string includeProperties = "");
        PostTag GetFirst(
            Expression<Func<PostTag, bool>> filter = null,
            string includeProperties = "");


    }
}
