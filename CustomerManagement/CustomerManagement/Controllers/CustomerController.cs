using CustomerManagement.Models;
using CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService customerService;

    public CustomerController(CustomerService customerService)
    {
        this.customerService = customerService;
    }

    [HttpGet]
    public ActionResult<List<Customer>> GetCustomers()
    {
        var customers = customerService.GetCustomers();

        return Ok(customers);
    }

    [HttpPost("addrandom/{count}")]
    public ActionResult AddRandomCustomers(int count)
    {
        try
        {
            customerService.AddRandomCustomers(count);
            return Ok($"{count} random customers added successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add")]
    public ActionResult AddCustomers([FromBody] List<Customer> newCustomers)
    {
        try
        {
            customerService.AddCustomers(newCustomers);
            return Ok($"{newCustomers.Count} customers added successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
