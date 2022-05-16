using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Repositories;
using PomogatorAPI.Models;

namespace PomogatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomer customerRep = new CustomerRepository();

        [HttpPost]
        async public Task<ActionResult> Create()
        {
            try
            {
                await customerRep.PostASS();
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Create")]
        async public Task<ActionResult<Customer>> Create(string id, string telNum, string name)
        {
            try { 
                Customer customer = new Customer(id, telNum, name);
                await customerRep.PostAsync(customer);
                return Ok(customer);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        

    }
}
