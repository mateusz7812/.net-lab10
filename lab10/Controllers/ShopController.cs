using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using lab10.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using lab10.Models;
using lab10.Data;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace lab10.Controllers
{
    [Authorize(Roles = "Client")]
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IRepository<Article> _repository;

        public ShopController(ILogger<HomeController> logger, MyDbContext context, IRepository<Article> repository)
        {
            _logger = logger;
            this._context = context;
            _repository = repository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? CategoryId)
        {
            var myDbContext = _repository.Get(null, 10, a => a.CategoryId == CategoryId || !CategoryId.HasValue);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name", CategoryId);
            return View(myDbContext);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Cart()
        {
            string cookieValueFromReq = Request.Cookies["cart"];
            if(cookieValueFromReq is null){
                return View(new List<CartItem>());
            }
            var cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(cookieValueFromReq);
            var articles = cartData.Keys.Select(key => _context.Article.Include(a => a.Category).SingleOrDefault(a => a.ArticleId == key)).Where(a=> a!=null);
            var cartItems = articles.Select(a => new CartItem(){Article = a, Quantity = cartData[a.ArticleId]});
            return View(cartItems.ToList());
        }

        public async Task<IActionResult> Order()
        {
            string cookieValueFromReq = Request.Cookies["cart"];
            if(cookieValueFromReq is null){
                return View(new List<CartItem>());
            }
            var cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(cookieValueFromReq);
            var articles = cartData.Keys.Select(key => _context.Article.Include(a => a.Category).SingleOrDefault(a => a.ArticleId == key)).Where(a=> a!=null);
            var cartItems = articles.Select(a => new CartItem(){Article = a, Quantity = cartData[a.ArticleId]});
            var order = new Order(){
                CartItems = cartItems.ToList()
            };
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([Bind("")] Order order)
        {
            Console.Out.WriteLine(order);
            if (ModelState.IsValid)
            {
                return View("OrderConfirmation", order);
            }
            return View(order);
        }
    }
}