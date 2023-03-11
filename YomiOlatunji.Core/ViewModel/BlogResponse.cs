using System.ComponentModel.DataAnnotations;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.Core.ViewModel
{
    public class BlogResponse
    {
        [Key]
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Excerpt { get; set; }
        public string Slug { get; set; }
        public string? HeaderImage { get; set; }
        public bool CanComment { get; set; } = true;
        public DateTimeOffset? PublishDate { get; set; }
        public string? CreateBy { get; set; }
        public IList<PostTag> Tags { get; set; } = new List<PostTag>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<RelatedBlog> RelatedBlogs { get; set; } = new List<RelatedBlog>();
    }
    public class RelatedBlog
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Excerpt { get; set; }
        public string Slug { get; set; }
        public string? HeaderImage { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        public string? CreateBy { get; set; }
    }
}
