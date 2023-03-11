using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class Tag : AuditTrail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public IList<PostTag> Tags { get; set; } = new List<PostTag>();
    }
}