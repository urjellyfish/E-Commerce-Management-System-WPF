using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;

namespace E_CommerceManagementSystem.Business.Services
{
    public class CategoryService
    {
        private CategoryRepository _repo = new();
        
        public List<Category> GetAll()
        {
            return _repo.GetAll();
        }



    }
}
