using E_CommerceManagementSystem.Repository.Entities;

namespace E_CommerceManagementSystem.Business.Services.CustomerServ
{
    public interface ICustomerService
    {
        bool IsAuth(string email, string password);
        void Create(Customer customer);
    }
}
