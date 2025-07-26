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

        public List<Category> GetAll()
        {
            _context = new();
            return _context.Categories.ToList();
        }
    }
}
