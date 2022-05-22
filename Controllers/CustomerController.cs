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

        private readonly ICustomer _customerRep;

        public CustomerController (ICustomer customerRep)
        {
            _customerRep = customerRep;
        }



        [HttpPost]
        async public Task<ActionResult> Create()
        {
            try
            {
                await _customerRep.PostASS();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        async public Task<ActionResult<List<Customer>>> Create(string id, string telNum, string name)
        {
            try { 
                await _customerRep.PostAsync(id, telNum, name);
                return Ok(_customerRep.Customers);
            }
            catch
            {
                return BadRequest();
            }
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

        [HttpDelete("{id}")]
        async public Task<ActionResult<List<Customer>>> Delete(string id)
        {
            try
            {
                await _customerRep.DeleteAsync(id);
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
