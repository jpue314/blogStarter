#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JPFinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JPFinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(b => b.User).OrderByDescending(c => c.BlogTimeStamp).AsNoTracking().ToListAsync();
            return View(blogs);
        }

        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            var blogsForUser = await _context.Blogs.Where(b => b.User.Id == currentUser.Id).ToListAsync();
            return View(blogsForUser);
        }


        // GET: Blog/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _usermanager.GetUserAsync(User);

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id && m.User != null && m.User.Id == currentUser.Id);
            if (blog == null)
            {
                return RedirectToAction("Manage");
            }

            return View(blog);
        }

        // GET: Blog/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,BlogTitle,BlogContent,BlogTimeStamp")] Blog blog)
        {

            if (ModelState.IsValid)
            {
                var currentUser = await _usermanager.GetUserAsync(User);
                blog.User = currentUser;
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }
        // GET: Blog/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Manage");
            }

            var currentUser = await _usermanager.GetUserAsync(User);
            var blog = await _context.Blogs
    .FirstOrDefaultAsync(m => m.BlogId == id && m.User != null && m.User.Id == currentUser.Id);

            if (blog == null)
            {
                return RedirectToAction("Manage");
            }
            return View(blog);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogTitle,BlogContent,BlogTimeStamp")] Blog blog)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            if (id != blog.BlogId)
            {
                return RedirectToAction("Manage");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blog/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Manage");
            }
            var currentUser = await _usermanager.GetUserAsync(User);

            var blog = await _context.Blogs
    .FirstOrDefaultAsync(m => m.BlogId == id && m.User != null && m.User.Id == currentUser.Id);
            if (blog == null)
            {
                return RedirectToAction("Manage");
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}

// public async Task<IActionResult> Index()
// {
//     var blogs = await _context.Blogs.Include(t => t.User).OrderByDescending(b => b.BlogTimeStamp).AsNoTracking().ToListAsync();
//     return View(blogs);
// }
