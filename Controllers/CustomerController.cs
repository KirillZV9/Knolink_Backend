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

        public CustomerController (ICustomer customerRep)
        {
            _customerRep = customerRep;
        }


        [HttpGet("{id}")]
        async public Task<ActionResult<List<Customer>>> Get(string id)
        {
            var identity = User.Identity as ClaimsIdentity;

            if (AuthService.GetUserId(identity) == id)
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

            return Forbid();
        }

        [HttpPut]
        async public Task<ActionResult<List<Customer>>> Put(Customer customerUpdated)
        {
            var identity = User.Identity as ClaimsIdentity;

            if (AuthService.GetUserId(identity) == customerUpdated.Id)
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

            return Forbid();
        }
        
        

    }
}
