using CustomerManagement.Interfaces;
using CustomerManagement.Models;
using CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(
            ICustomerService customerService,
            ILogger<CustomerController> logger)
        {
            this._customerService = customerService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Customer>> GetCustomers()
        {
            try
            {
                var customers = _customerService.GetCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetCustomers: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("addrandom/{count}")]
        public ActionResult AddRandomCustomers(int count)
        {
            try
            {
                if (count <= 0)
                {
                    logger.LogWarning("Invalid count provided in AddRandomCustomers");
                    return BadRequest("Count must be greater than 0");
                }

                _customerService.AddRandomCustomers(count);
                return Ok($"{count} random customers added successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in AddRandomCustomers: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public ActionResult AddCustomers([FromBody] List<Customer> newCustomers)
        {
            try
            {
                if (newCustomers == null || !newCustomers.Any())
                {
                    logger.LogWarning("No customers provided for addition in AddCustomers");
                    return BadRequest("List of Customers is empty");
                }

                _customerService.AddCustomers(newCustomers);
                return Ok($"{newCustomers.Count} customers added successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in AddCustomers: {ex}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
