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
        private ClaimsIdentity? Identity { get; set; }
        private string Id {get{return AuthService.GetUserId(Identity);}}

        public CustomerController (ICustomer customerRep)
        {
            _customerRep = customerRep;
        }


        [HttpGet]
        async public Task<ActionResult<List<Customer>>> Get()
        {
            Identity = User.Identity as ClaimsIdentity;

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
            Identity = User.Identity as ClaimsIdentity;

            if (Id == customerUpdated.Id)
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
