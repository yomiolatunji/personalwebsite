using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.DataSource.Interface;

namespace YomiOlatunji.DataSource.Repository
{
    public class PostTagRepository: IPostTagRepository
    {

        internal ApplicationDbContext context;
        internal DbSet<PostTag> dbSet;

        public PostTagRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<PostTag>();
        }

        public virtual void Add(PostTag model)
        {
            try
            {
                dbSet.AddAsync(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public virtual void Add(IEnumerable<PostTag> listEntity)
        {
            dbSet.AddRangeAsync(listEntity);
        }

        public virtual void Delete(long postId,long tagId)
        {
            PostTag entityToDelete = dbSet.Where(a=>a.PostId==postId&&a.TagId==tagId).FirstOrDefault();
            Delete(entityToDelete);
        }

        public virtual void Delete(PostTag entityToDelete)
        {
            dbSet.Attach(entityToDelete);
            //entityToDelete.IsDeleted = true;
            //entityToDelete.UpdateTime = DateTimeOffset.Now;
            //context.Entry(entityToDelete).State = EntityState.Modified;
            context.Entry(entityToDelete).State = EntityState.Deleted;
        }

        public virtual async Task<IEnumerable<PostTag>> Get(Expression<Func<PostTag, bool>> filter = null, Func<IQueryable<PostTag>, IOrderedQueryable<PostTag>> orderBy = null, string includeProperties = "")
        {
            IQueryable<PostTag> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


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
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToList();
            }
        }
        public virtual PostTag GetFirst(Expression<Func<PostTag, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<PostTag> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //var properties = includeProperty.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public virtual async Task<PostTag> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual void Update(PostTag model)
        {
            dbSet.Attach(model);
            context.Entry(model).State = EntityState.Modified;
        }

    }
}
