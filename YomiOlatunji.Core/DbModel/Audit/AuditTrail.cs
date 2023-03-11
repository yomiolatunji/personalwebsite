using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.Core.DbModel.Audit
{
    public class AuditTrail
    {
        public AuditTrail()
        {
            CreateTime = DateTimeOffset.Now;
        }

        public DateTimeOffset CreateTime { get; set; }

        public string? CreateBy { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
        public string? UpdateBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
