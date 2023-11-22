using CustomerManagement.Models;
using CustomerManagement.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests;

[TestClass]
public class CustomerServiceTests
{
    [TestMethod]
    public void AddRandomCustomers_ShouldAddCustomers()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CustomerService>();
        var service = new CustomerService(logger);

        // Act
        service.AddRandomCustomers(5);

        // Assert
        var customers = service.GetCustomers();
        Assert.AreNotEqual(0, customers.Count);
    }

    [TestMethod]
    public void AddCustomers_WithInvalidCustomer_ShouldThrowException()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CustomerService>();
        var service = new CustomerService(logger);
        var invalidCustomer = new Customer(); // Missing required fields

        // Act and Assert
        Assert.ThrowsException<Exception>(() => service.AddCustomers(new List<Customer> { invalidCustomer }));
    }

    [TestMethod]
    public void AddCustomers_WithInvalidAge_ShouldThrowException()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<CustomerService>();
        var service = new CustomerService(logger);
        var invalidCustomer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 16, // Invalid age (below 18)
            Id = 1
        };

        // Act and Assert
        Assert.ThrowsException<Exception>(() => service.AddCustomers(new List<Customer> { invalidCustomer }));
    }
}