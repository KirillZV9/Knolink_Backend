using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Repositories;
using PomogatorAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace PomogatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "customer")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomer _customerRep;

        public CustomerController (ICustomer customerRep)
        {
            _customerRep = customerRep;
        }


        [HttpGet("{id}")]
        async public Task<ActionResult<List<Customer>>> Get(string id)
        {
            try
            {
                await _customerRep.GetAsync(id);
                return Ok(_customerRep.Customers);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        async public Task<ActionResult<List<Customer>>> Put(Customer customerUpdated)
        {
            try
            {
                await _customerRep.UpdateAsync(customerUpdated);
                return Ok(_customerRep.Customers);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        

    }
}
