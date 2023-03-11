using YomiOlatunji.Core.DbModel.ActivityLog;

namespace YomiOlatunji.DataSource.Interface
{
    public interface ILogRepository
    {
        Task AddErrorLogFromException(Exception exception);
        Task AddErrorLog(ErrorLog errorLog);
        Task UpdateErrorLog(ErrorLog errorLog);
        Task<IEnumerable<ErrorLog>> GetAllErrorLog();
        Task<IEnumerable<ErrorLog>> GetErrorLogByDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ErrorLog>> GetErrorLogByFilter(ErrorLog errorLog);
        Task<ErrorLog> GetErrorLogById(string id);
    }
}
