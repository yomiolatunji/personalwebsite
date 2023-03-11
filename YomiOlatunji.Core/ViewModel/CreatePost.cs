using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YomiOlatunji.Core.DbModel.Post;

namespace YomiOlatunji.Core.ViewModel
{
    public class CreatePost
    {
        //public CreatePost(int categoryId, string title, string content)
        //{
        //    CategoryId = categoryId;
        //    Title = title;
        //    Content = content;
        //}

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
