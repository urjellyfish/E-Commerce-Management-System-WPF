using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;

namespace E_CommerceManagementSystem.Business.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _repo = new();

        public bool IsAuth(string email, string password)
        {
            return _repo.IsAuth(email, password);
        }

        public void Create(Customer customer)
        {
            _repo.Create(customer);
        }

        public Customer? GetCustomerByEmail(string email)
        {
            return _repo.Read().FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
        }
    }
}
