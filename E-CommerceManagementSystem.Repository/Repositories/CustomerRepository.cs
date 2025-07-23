using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace E_CommerceManagementSystem.Repository.Repositories
{
    public class CustomerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder = new();
        private AppDbContext _context;

        public CustomerRepository()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _optionsBuilder?.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public string? GetEmail()
        {
            return _configuration["AdminAccount:Email"];
        }

        public string? GetPassword()
        {
            return _configuration["AdminAccount:Password"];
        }

        public bool IsAuth(string email, string password)
        {
            _context = new(_optionsBuilder.Options);

            return _context.Customers.FirstOrDefault(c => c.Email.ToLower() == email.ToLower() && c.Password == password) != null ||
                email.ToLower() == GetEmail()?.ToLower() && password == GetPassword();
        }

        public void Create(Customer customer)
        {
            _context = new(_optionsBuilder.Options);

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public List<Customer> Read()
        {
            _context = new(_optionsBuilder.Options);
            return _context.Customers.ToList();
        }
    }
}
