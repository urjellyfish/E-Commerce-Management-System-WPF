using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Business.Services
{
    public class OrderService
    {

        private OrderRepository orderRepository = new();

        public List<Order> GetAll()
        {
            return orderRepository.GetAll();
        }

        public void Create(Order project)
        {
            orderRepository.Create(project);
        }

        public void Update(Order project)
        {
            orderRepository.Update(project);
        }

        public void Delete(Order project)
        {
            orderRepository.Delete(project);
        }

        public List<Order> Search(string keyword)
        {
            var searchList = orderRepository.GetAll();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return searchList;
            }
            else
            {
                return searchList
                    .Where(p => p.Customer.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                p.Status.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

    }
}
