using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class LinkType : AuditTrail
    {
        [Key]
        public short Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Icon { get; set; }
        public string ColorCode { get; set; }
    }

    public class PostLink
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Post")]
        public long PostId { get; set; }

        public Post Post { get; set; }

        [ForeignKey("LinkType")]
        public short LinkTypeId { get; set; }

        public LinkType LinkType { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }

}