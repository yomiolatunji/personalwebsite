using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.Core.DbModel.Audit
{
    public class AuthAuditTrail : AuditTrail
    {
        public DateTimeOffset? AuthorizeTime { get; set; }
        public string? AuthorizedBy { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
