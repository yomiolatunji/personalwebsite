using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YomiOlatunji.Core.DbModel.Post;
using YomiOlatunji.Core.ViewModel;
using YomiOlatunji.DataSource;
using YomiOlatunji.DataSource.Services.Interfaces;

namespace YomiOlatunji.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        [TempData]
        public string statusMessage { get; set; }

        public PostController(ApplicationDbContext context, IMapper mapper, IPostService postService, ICategoryService categoryService)
        {
            _context = context;
            _mapper = mapper;
            _postService = postService;
            _categoryService = categoryService;
        }
        // GET: PostController
        public async Task<ActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Category).ToListAsync();
            return View(posts);
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(long id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(a => a.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET: PostController/Create
        public async Task<ActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePost post)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "Id", "Name", post.CategoryId);
                    return View();
                }
                var mappedPost = _mapper.Map<Post>(post);
                var userName = User.FindFirstValue(ClaimTypes.Name);
                var saved = await _postService.AddPost(mappedPost, userName);
                if (saved)
                {
                    statusMessage = "Post successfully added";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategories(), "Id", "Name", post.CategoryId);
                return View();
            }
        }

        // GET: PostController/Edit/5
        public async Task<ActionResult> Edit(long id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, Post post)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
                    return View(post);
                }

                _context.Attach(post).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
                return View(post);
            }
        }

        private bool PostExists(long id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<ActionResult> Publish(long id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish(long id, Post post)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View(post);
                //}

                var userName = User.FindFirstValue(ClaimTypes.Name);
                var saved = await _postService.PublishPost(post.Id, userName);

                if (saved)
                {
                    statusMessage = "Post successfully published";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new JsonResult(null);

            UploadFileResult result = new UploadFileResult();

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        GetUniqueFileName(file.FileName));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            result.Location = path;
            return new JsonResult(result);
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        // GET: PostController/Delete/5
        public async Task<ActionResult> Delete(long id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            
            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Post post1)
        {
            try
            {
                if (_context.Posts == null)
                {
                    return NotFound();
                }
                var post = await _context.Posts.FindAsync(id);

                if (post != null)
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post1);
            }
        }
    }
}
