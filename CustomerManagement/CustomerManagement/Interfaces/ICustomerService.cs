using CustomerManagement.Models;

namespace CustomerManagement.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers();

        void AddRandomCustomers(int count);

        void AddCustomers(List<Customer> newCustomers);

        List<Customer> LoadPersistedData();
    }
}
