using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.ActivityLog
{
    public class ErrorLog : AuditTrail
    {
        [Key]
        public string ErrorId { get; set; }
        public string ErrorType { get; set; }
        public string Message { get; set; }
        public string SessionId { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string UserId { get; set; }
        public DateTime ErrorTime { get; set; }
    }
}
