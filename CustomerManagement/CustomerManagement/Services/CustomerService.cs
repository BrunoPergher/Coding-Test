using CustomerManagement.Models;
using Newtonsoft.Json;

namespace CustomerManagement.Services;

public class CustomerService
{
    private const string DataFilePath = "customers.json";
    private readonly string[] firstNames = { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
    private readonly string[] lastNames = { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
    private readonly List<Customer> customers;
    private readonly Random random = new Random();

    public CustomerService()
    {
        customers = LoadPersistedData() ?? new List<Customer>();
    }

    public List<Customer> GetCustomers()
    {
        return new List<Customer>(customers);
    }

    public void AddRandomCustomers(int count)
    {
        var newCustomers = new List<Customer>();

        for (int i = 0; i < count; i++)
        {
            var newCustomer = GenerateRandomCustomer();
            ValidateCustomer(newCustomer);
            InsertSorted(newCustomer);
            newCustomers.Add(newCustomer);
        }

        PersistData();
    }

    public void AddCustomers(List<Customer> newCustomers)
    {
        foreach (var newCustomer in newCustomers)
        {
            ValidateCustomer(newCustomer);
            InsertSorted(newCustomer);
        }

        PersistData();
    }

    public List<Customer> LoadPersistedData()
    {
        if (File.Exists(DataFilePath))
        {
            var jsonData = File.ReadAllText(DataFilePath);
            return JsonConvert.DeserializeObject<List<Customer>>(jsonData);
        }

        return null;
    }

    #region Private Methods
    private Customer GenerateRandomCustomer()
    {
        var firstName = firstNames[random.Next(firstNames.Length)];
        var lastName = lastNames[random.Next(lastNames.Length)];
        var age = random.Next(10, 91);
        var id = customers.Count > 0 ? customers.Max(x => x.Id) + 1 : 1;

        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Age = age,
            Id = id
        };
    }

    private void ValidateCustomer(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.LastName))
        {
            throw new Exception("First name and last name must be provided.");
        }

        if (customer.Age <= 18)
        {
            throw new Exception("Age must be above 18.");
        }

        if (customers.Exists(x => x.Id == customer.Id))
        {
            throw new Exception("ID has already been used before.");
        }
    }

    private void InsertSorted(Customer newCustomer)
    {
        int index = 0;

        while (index < customers.Count &&
               string.Compare(newCustomer.LastName, customers[index].LastName, StringComparison.Ordinal) > 0)
        {
            index++;
        }

        while (index < customers.Count &&
               string.Compare(newCustomer.LastName, customers[index].LastName, StringComparison.Ordinal) == 0 &&
               string.Compare(newCustomer.FirstName, customers[index].FirstName, StringComparison.Ordinal) > 0)
        {
            index++;
        }

        customers.Insert(index, newCustomer);
    }

    private void PersistData()
    {
        var jsonData = JsonConvert.SerializeObject(customers, Formatting.Indented);
        File.WriteAllText(DataFilePath, jsonData);
    }

    #endregion
}
