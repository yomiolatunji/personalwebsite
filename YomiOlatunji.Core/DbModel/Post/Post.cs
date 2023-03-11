using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YomiOlatunji.Core.DbModel.Audit;

namespace YomiOlatunji.Core.DbModel.Post
{
    public class Post : AuthAuditTrail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Content { get; set; }
        [MaxLength(200)]
        public string? Excerpt { get; set; }
        [MaxLength(200)]
        public string Slug { get; set; }
        public string? HeaderImage { get; set; }
        public bool CanComment { get; set; } = true;
        public bool IsPublished { get; set; }
        public bool IsArchived { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        public IList<PostTag> Tags { get; set; } = new List<PostTag>();
        public IList<Comment> Comments { get; set; }
    }

    public class Comment : AuthAuditTrail
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Post")]
        public long PostId { get; set; }

        public Post Post { get; set; }
        [MaxLength(200)]
        public string AuthorName { get; set; }
        [MaxLength(200)]
        public string? AuthorEmail { get; set; }
        public bool? IsAnonymous { get; set; }
        public string Message { get; set; }
        public long ParentId { get; set; }
    }

    public class PostTag
    {
        //[Key]
        //public Guid Id { get; set; } = new Guid();

        [ForeignKey("Post")]
        public long PostId { get; set; }

        public Post Post { get; set; }

        [ForeignKey("Tag")]
        public long TagId { get; set; }

        public Tag Tag { get; set; }

        [NotMapped]
        public string Name { get; set; }
    }
}