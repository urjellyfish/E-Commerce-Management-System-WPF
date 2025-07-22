using E_CommerceManagementSystem.Repository.Entities;

namespace E_CommerceManagementSystem.Repository.Repositories.CustomerRepo
{
    public interface ICustomerRepository
    {
        bool IsAuth(string email, string password);
        void Create(Customer customer);
    }
}
