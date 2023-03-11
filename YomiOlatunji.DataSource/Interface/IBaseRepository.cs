using System.Linq.Expressions;
using YomiOlatunji.Core;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.DataSource.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : AuditTrail
    {
        void Add(TEntity model);

        void Add(IEnumerable<TEntity> listEntity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity model);

        Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        TEntity GetFirst(
            Expression<Func<TEntity, bool>>? filter = null,
            string includeProperties = "");

        Task<TEntity> GetById(object id);

        Task<PageModel<TEntity>> GetPage(Expression<Func<TEntity, bool>>? filter = null,
            int pageNumber = 1, int pageSize = 20,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        Task<bool> SaveChanges();
    }
}
