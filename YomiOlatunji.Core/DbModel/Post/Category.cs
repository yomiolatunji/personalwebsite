using System.ComponentModel.DataAnnotations;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class Category : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        [MaxLength(10)]
        public string? ColorCode { get; set; }

        public string? HeaderImage { get; set; }
        public bool DedicatedCategory { get; set; }
    }
}