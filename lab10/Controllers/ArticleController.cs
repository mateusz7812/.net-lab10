using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab10.Data;
using lab10.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace lab10
{

    [Authorize(Roles = "Admin")]
    public class ArticleController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<Article> _repository;

        public ArticleController(IWebHostEnvironment environment, IRepository<Article> repository, MyDbContext context)
        {
            _environment = environment;
            this._repository = repository;
            _context = context;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            //var myDbContext = _context.Article.Include(a => a.Category);
            //return View(await myDbContext.ToListAsync());
            return View(_repository.Get());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _repository[id.Value];
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name");
            return View();
        }

        public async Task<String> SaveFile(IFormFile file)
        {
            string uploads = Path.Combine(_environment.WebRootPath, "upload");
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploads, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public void RemoveFile(String name)
        {
            string uploads = Path.Combine(_environment.WebRootPath, "upload");
            string filePath = Path.Combine(uploads, name);
            System.IO.File.Delete(filePath);
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Name,Money,CategoryId, Category,Image")] ArticleViewModel articleViewModel)
        {
            IFormFile file = articleViewModel.Image;
            Article article = new Article()
            {
                ArticleId = articleViewModel.ArticleId,
                CategoryId = articleViewModel.CategoryId,
                Money = articleViewModel.Money,
                Name = articleViewModel.Name
            };
            if (ModelState.IsValid)
            {
                if (file is not null)
                {
                    String fileName = await SaveFile(file);
                    article.ImageName = fileName;
                }
                _repository.Add(article);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Name,Money,CategoryId, ImageName")] Article article, [Bind("ArticleId,Name,Money,CategoryId,Category,Image")] ArticleViewModel articleViewModel)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (articleViewModel.Image is not null)
                    {
                        if (article.ImageName is not null)
                        {
                            RemoveFile(article.ImageName);
                        }

                        String fileName = await SaveFile(articleViewModel.Image);
                        article.ImageName = fileName;
                    }
                    _repository.Update(article);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _repository[id.Value];
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            _repository.Delete(id);
            if (article.ImageName is not null)
                RemoveFile(article.ImageName);
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }
    }
}
