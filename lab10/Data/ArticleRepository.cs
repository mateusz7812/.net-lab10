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


namespace lab10.Data
{
    public class ArticleRepository : IRepository<Article>
    {
        private readonly MyDbContext _context;
        public ArticleRepository(MyDbContext context)
        {
            _context = context;
        }
        
        public Article this[int id] =>
            _context.Article
                .Include(a => a.Category)
                .FirstOrDefault(m => m.ArticleId == id);
        public IEnumerable<Article> Get(int? start_id, int number, Func<Article,bool> filter) => 
            _context.Article
                .Where(a => a.ArticleId > (start_id ?? 0))
                .OrderBy(a => a.ArticleId)
                .Include(a => a.Category)
                .Where(filter)
                .Take(number)
                .ToList();
        public Article Add(Article item){
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }
        public Article Update(Article item){
            _context.Update(item);
            _context.SaveChanges();
            return item;
        }
        public bool Delete(int id){
            return true;
            _context.Article.Remove(_context.Article.Find(id));
            _context.SaveChanges();
        }
    }

}