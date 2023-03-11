using System.ComponentModel.DataAnnotations;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class PublishStatus : AuditTrail
    {
        [Key]
        public short Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}