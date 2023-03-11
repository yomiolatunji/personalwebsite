using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Audit;
using YomiOlatunji.Core;
using YomiOlatunji.DataSource.Interface;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.DataSource.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : AuditTrail
    {
        internal ApplicationDbContext context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity model)
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

        public virtual void Add(IEnumerable<TEntity> listEntity)
        {
            dbSet.AddRangeAsync(listEntity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            dbSet.Attach(entityToDelete);
            entityToDelete.IsDeleted = true;
            entityToDelete.UpdateTime = DateTimeOffset.Now;
            context.Entry(entityToDelete).State = EntityState.Modified;
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

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
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToList();
            }
        }
        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

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
                    //var properties = includeProperty.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    query = query.Include(includeProperty);
                }
            }

            return query?.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<PageModel<TEntity>> GetPage(Expression<Func<TEntity, bool>>? filter = null, int intPageIndex = 1, int intPageSize = 20, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

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

            return new PageModel<TEntity>() { DataCount = (int)totalCount, PageCount = pageCount, PageNumber = intPageIndex, PageSize = intPageSize, Data = list };
        }

        public virtual void Update(TEntity model)
        {
            dbSet.Attach(model);
            context.Entry(model).State = EntityState.Modified;

            
        }
        public virtual async Task<bool> SaveChanges()
        {
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
