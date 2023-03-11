using System.ComponentModel.DataAnnotations;

namespace YomiOlatunji.Core.ViewModel
{
    public class AddComment
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
