using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Printing;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;
using YomiOlatunji.DataSource.Interface;
using YomiOlatunji.DataSource.Services.Interfaces;
using YomiOlatunji.Models;

namespace YomiOlatunji.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IContactMessageService _contactMessageService;

        public HomeController(ILogger<HomeController> logger, IPostService postService, IContactMessageService contactMessageService)
        {
            _logger = logger;
            _postService = postService;
            _contactMessageService = contactMessageService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Blogs(int pageNumber = 1, int pageSize = 20)
        {
            var post = await _postService.GetPost(pageNumber, pageSize, a => a.IsPublished == true, a => a.OrderByDescending(s => s.PublishDate), "Category");
            return View(post);
        }

        public async Task<IActionResult> Blog(string slug)
        {
            var post = await _postService.GetPostBySlug(slug);
            if (post == null)
            {
                return NotFound();
            }

            var blogResponse = new BlogResponse
            {
                CanComment = post.CanComment,
                Category = post.Category,
                CategoryId = post.CategoryId,
                Content = post.Content,
                Excerpt = post.Excerpt,
                HeaderImage = post.HeaderImage,
                Id = post.Id,
                PublishDate = post.PublishDate,
                Slug = post.Slug,
                Title = post.Title,
                Tags = post.Tags,
                CreateBy = post.CreateBy,
                Comments = post.Comments,
            };
            return View(blogResponse);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(AddContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return View(message);
            }
            var response = await _contactMessageService.AddMessage(message);
            if (response)
            {
                TempData["success"] = "Message Sent";
                message = new AddContactMessage();
            }
            else
            {
                TempData["error"] = "Error sending message";
            }
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> SaveComment(AddComment comment)
        {
            if (ModelState.IsValid)
            {
                bool response = await _postService.AddComment(comment);
                if (response)
                {
                    return RedirectToAction("Blog", new { slug = comment.Slug });
                }
            }

            return RedirectToAction("Blog", new { slug = comment.Slug });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}