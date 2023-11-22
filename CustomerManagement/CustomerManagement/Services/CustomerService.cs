using CustomerManagement.Interfaces;
using CustomerManagement.Models;
using Newtonsoft.Json;

namespace CustomerManagement.Services;

public class CustomerService : ICustomerService
{
    private const string DataFilePath = "customers.json";
    private readonly string[] firstNames = { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
    private readonly string[] lastNames = { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
    private readonly List<Customer> customers;
    private readonly Random random = new Random();
    private readonly ILogger<CustomerService> logger;

    public CustomerService(ILogger<CustomerService> logger)
    {
        this.logger = logger;
        customers = LoadPersistedData() ?? new List<Customer>();
    }

    public List<Customer> GetCustomers()
    {
        logger.LogInformation("GetCustomers method called");
        return new List<Customer>(customers);
    }

    public void AddRandomCustomers(int count)
    {
        logger.LogInformation($"AddRandomCustomers method called with count: {count}");

        try
        {
            if (count <= 0)
            {
                logger.LogWarning("Invalid count provided in AddRandomCustomers");
                throw new ArgumentException("Count must be greater than 0");
            }

            var newCustomers = new List<Customer>();

            for (int i = 0; i < count; i++)
            {
                var newCustomer = GenerateRandomCustomer();
                ValidateCustomer(newCustomer);
                InsertSorted(newCustomer);
                newCustomers.Add(newCustomer);
            }

            PersistData();

            logger.LogInformation($"{count} random customers added successfully");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddRandomCustomers: {ex}");
            throw;
        }
    }

    public void AddCustomers(List<Customer> newCustomers)
    {
        logger.LogInformation($"AddCustomers method called with {newCustomers.Count} customers");

        try
        {
            if (newCustomers == null || newCustomers.Count == 0)
            {
                logger.LogWarning("No customers provided for addition in AddCustomers");
                throw new ArgumentException("List of customers is empty");
            }

            foreach (var newCustomer in newCustomers)
            {
                ValidateCustomer(newCustomer);
                InsertSorted(newCustomer);
            }

            PersistData();

            logger.LogInformation($"{newCustomers.Count} customers added successfully");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddCustomers: {ex}");
            throw;
        }
    }

    public List<Customer> LoadPersistedData()
    {
        logger.LogInformation("LoadPersistedData method called");

        try
        {
            if (File.Exists(DataFilePath))
            {
                var jsonData = File.ReadAllText(DataFilePath);
                return JsonConvert.DeserializeObject<List<Customer>>(jsonData);
            }

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in LoadPersistedData: {ex}");
            throw;
        }
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