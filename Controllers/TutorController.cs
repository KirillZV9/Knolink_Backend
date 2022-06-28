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
    [Route("[controller]")]
    [Authorize(Roles = "tutor")]
    public class TutorController : ControllerBase
    {
        private readonly ITutor _tutorRep;
        private ClaimsIdentity? Identity { get; set; }
        private string Id { get { return AuthService.GetUserId(Identity); } }

        public TutorController(ITutor tutorRep)
        {
            _tutorRep = tutorRep;
        }


        [HttpGet]
        async public Task<ActionResult<List<Tutor>>> Get()
        {
            Identity = User.Identity as ClaimsIdentity;

            try
            {
                await _tutorRep.GetAsync(Id);
                return Ok(_tutorRep.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut]
        async public Task<ActionResult<List<Tutor>>> Put(Tutor tutorUpdated)
        {
            Identity = User.Identity as ClaimsIdentity;

            if (Id == tutorUpdated.Id)
                return Forbid();

            try
            {
                await _tutorRep.UpdateAsync(tutorUpdated);
                return Ok(_tutorRep.Tutors);
            }
            catch
            {
                return BadRequest();
            }
            
        }

    }
}
