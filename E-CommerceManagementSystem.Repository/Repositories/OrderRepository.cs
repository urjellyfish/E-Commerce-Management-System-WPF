using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Repository.Repositories
{
    public class OrderRepository
    {

        private readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder = new();
        private AppDbContext context;

        public List<Order> GetAll()
        {
            context = new();
            return context.Orders.Include(o => o.Products).Include(o => o.Customer).ToList();
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            context = new();
            return context.Orders
                .Include(o => o.Products)
                .Include(o => o.Customer)
                .Where(o => o.Customer.CustomerID == customerId)
                .ToList();
        }

        public Order? Create(Order order)
        {
            context = new();
            context.Orders.Add(order);
            context.SaveChanges();
            return order;
        }

        public void Update(Order order)
        {
            context = new();
            context.Orders.Update(order);
            context.SaveChanges();
        }

        public void Delete(Order order)
        {
            context = new();
            context.Orders.Remove(order);
            context.SaveChanges();
        }

    }
}
