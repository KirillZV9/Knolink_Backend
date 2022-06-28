using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Repositories;
using PomogatorAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PomogatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "customer")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomer _customerRep;
        private string Id { get{return User.FindFirstValue(ClaimTypes.NameIdentifier); }}

        public CustomerController (ICustomer customerRep)
        {
            _customerRep = customerRep;
        }


        [HttpGet]
        async public Task<ActionResult<List<Customer>>> Get()
        {

            try
            {
                await _customerRep.GetAsync(Id);
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

            if (Id != customerUpdated.Id)
                return Forbid();

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
