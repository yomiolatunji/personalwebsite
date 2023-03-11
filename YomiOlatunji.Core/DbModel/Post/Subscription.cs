using System;
using System.ComponentModel.DataAnnotations;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class Subscription : AuditTrail
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string EmailAddress { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public bool Subscribed { get; set; } = true;
        public DateTime? UnsubscribeDate { get; set; }

        [MaxLength(500)]
        public string UnsubscribeReason { get; set; }
    }
}