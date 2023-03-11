using Microsoft.EntityFrameworkCore;
using YomiOlatunji.Core.DbModel.ActivityLog;
using YomiOlatunji.DataSource.Interface;

namespace YomiOlatunji.DataSource.Repository
{
    public class LogRepository : ILogRepository
    {
        internal ApplicationDbContext context;
        internal DbSet<ErrorLog> dbSet;
        public LogRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<ErrorLog>();
        }

        public async Task AddErrorLogFromException(Exception exception)
        {
            ErrorLog errorLog = new ErrorLog()
            {
                ErrorType = exception.GetType().FullName,
                Message = exception.Message,
                SessionId = "",
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                UserId = "",
                CreateTime = DateTime.Now,
                ErrorTime = DateTime.Now,
                IsDeleted = false
            };
            await AddErrorLog(errorLog);
        }
        public async Task AddErrorLog(ErrorLog errorLog)
        {
            errorLog.ErrorId = Guid.NewGuid().ToString();
            try
            {
                await dbSet.AddAsync(errorLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Task<IEnumerable<ErrorLog>> GetAllErrorLog()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ErrorLog>> GetErrorLogByDate(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ErrorLog>> GetErrorLogByFilter(ErrorLog errorLog)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorLog> GetErrorLogById(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateErrorLog(ErrorLog errorLog)
        {
            throw new NotImplementedException();
        }
    }
}
