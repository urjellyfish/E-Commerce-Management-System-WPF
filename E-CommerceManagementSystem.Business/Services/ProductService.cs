using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;

namespace E_CommerceManagementSystem.Business.Services
{
    public class ProductService
    {

        private ProductRepository _repo = new();

        public Product GetProductById(int id)
        {
            return _repo.GetProductById(id);
        }

        public List<Product> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Product> GetAllProductByOrderId(int orderId)
        {
            return _repo.GetAllProductByOrderId(orderId);
        }

        public void Add(Product p)
        {
            _repo.Add(p);
        }

        public Product? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void Update(Product p)
        {
            _repo.Update(p);
        }

        public void Delete(Product p)
        {
            _repo.Delete(p);
        }

        public List<Product> Search(string keyword)
        {
            return _repo.Search(keyword);
        }

        public int GetMaxId()
        {
            return _repo.GetMaxId();
        }

        public List<Product> FilterByCate(int cateID)
        {
            return _repo.FilterByCate(cateID);
        }
    }
}
