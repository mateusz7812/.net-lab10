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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly MyDbContext _context;
        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }
        
        public Category this[int id] =>
            _context.Category
                .FirstOrDefault(m => m.CategoryID == id);
        public IEnumerable<Category> Get(int? start_id, int number, Func<Category,bool> filter) => 
            _context.Category
                .Where(a => a.CategoryID > (start_id ?? 0))
                .OrderBy(a => a.CategoryID)
                .Where(filter)
                .Take(number)
                .ToList();
        public Category Add(Category item){
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }
        public Category Update(Category item){
            _context.Update(item);
            _context.SaveChanges();
            return item;
        }
        public bool Delete(int id)
        {
            if (_context.Article.Any(m => m.CategoryId == id))
            {
                return false;
            }
            _context.Category.Remove(_context.Category.Find(id));
            _context.SaveChanges();
            return true;
        }
    }

}