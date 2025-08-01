using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceManagementSystem.Repository.Entities;

namespace E_CommerceManagementSystem.Repository.Repositories
{
    public class CategoryRepository
    {
        private AppDbContext _context;

        public CategoryRepository()
        {
            _context = new AppDbContext();
        }

        public List<Category> GetAll()
        {
            _context = new();
            return _context.Categories.ToList();
        }

        public Category? GetById(int categoryId)
        {
            _context = new();
            return _context.Categories.Find(categoryId);
        }

        public void Add(Category category)
        {
            _context = new();
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context = new();
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int categoryId)
        {
            _context = new();
            var category = _context.Categories.Find(categoryId);
            if(category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
