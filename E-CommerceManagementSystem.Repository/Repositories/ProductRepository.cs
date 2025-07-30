using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceManagementSystem.Repository.Repositories
{
    public class ProductRepository
    {
        private readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder = new();
        private AppDbContext _context;

        public List<Product> GetAll()
        {
            _context = new();
            return _context.Products.Include("Category").ToList();
        }

        public void Add(Product p)
        {
            _context = new();
            _context.Add(p);
            _context.SaveChanges();
        }

        public void Update(Product p)
        {
            _context = new();
            _context.Update(p);
            _context.SaveChanges();
        }

        public void Delete(Product p)
        {
            _context = new();
            _context.Remove(p);
            _context.SaveChanges();
        }

        public int GetMaxId()
        {
            _context = new();
            if (!_context.Products.Any())
                return 0;
            return _context.Products.Max(p => p.ProductID);
        }

        public List<Product> Search(string keyword)
        {
            _context = new();
            keyword = keyword.ToLower().Trim();
            return _context.Products
                .Include("Category")
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .ToList();

        }

        public List<Product> FilterByCate(int caterId)
        {
            _context = new();
            return  _context.Products
                .Include("Category")
                .Where(p => p.CategoryID == caterId)
                .ToList();
        }
    }
}
