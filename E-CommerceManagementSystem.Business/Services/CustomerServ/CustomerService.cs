using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories.CustomerRepo;

namespace E_CommerceManagementSystem.Business.Services.CustomerServ
{
    public class CustomerService(ICustomerRepository _repo) : ICustomerService
    {
        public bool IsAuth(string email, string password)
        {
            return _repo.IsAuth(email, password);
        }

        public void Create(Customer customer)
        {
            _repo.Create(customer);
        }
    }
}
