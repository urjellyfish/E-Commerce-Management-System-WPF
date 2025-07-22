using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.Extensions.Configuration;

namespace E_CommerceManagementSystem.Repository.Repositories.CustomerRepo
{
    public class CustomerRepository(AppDbContext _context) : ICustomerRepository
    {
        public string? GetEmail()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return configuration["AdminAccount:Email"];
        }

        public string? GetPassword()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return configuration["AdminAccount:Password"];
        }

        public bool IsAuth(string email, string password)
        {
            return _context.Customers.FirstOrDefault(c => c.Email.ToLower() == email.ToLower() && c.Password == password) != null ||
                email.ToLower() == GetEmail()?.ToLower() && password == GetPassword();
        }

        public void Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}
