using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;
using E_CommerceManagementSystem.Business.DTO;

namespace E_CommerceManagementSystem.Business.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;
        
        public CategoryService()
        {
            _repo = new CategoryRepository();
        }

        public List<CategoryDTO> GetAll()
        {
            return _repo.GetAll().Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryID,
                Name = c.Name,
                Description = c.Description,
                Picture = c.Picture
            }).ToList();
        }

        public CategoryDTO? GetCategoryById(int id)
        {
            var categoryEntity = _repo.GetById(id);
            if (categoryEntity == null) return null;

            return new CategoryDTO
            {
                CategoryId = categoryEntity.CategoryID,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                Picture = categoryEntity.Picture
            };
        }

        public void AddCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            // Check for duplicate names
            if (_repo.GetAll().Any(c => c.Name.Equals(categoryDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A category with this name already exists.");
            }

            var categoryEntity = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Picture = categoryDto.Picture
            };
            _repo.Add(categoryEntity);
        }

        public void UpdateCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            var categoryEntity = new Category
            {
                CategoryID = categoryDto.CategoryId,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Picture = categoryDto.Picture
            };
            _repo.Update(categoryEntity);
        }

        public void DeleteCategory(int id)
        {
            _repo.Delete(id);
        }
    }
}
