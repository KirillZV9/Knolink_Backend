using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Repositories;
using PomogatorAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace PomogatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "tutor")]
    public class TutorController : ControllerBase
    {
        private readonly ITutor _tutorRep;

        public TutorController(ITutor tutorRep)
        {
            _tutorRep = tutorRep;
        }

        /*
        [HttpPost]
        async public Task<ActionResult> Create(string id, string telNum, string name)
        {
            try
            {
                await _tutorRep.PostAsync(id, telNum, name);
                return Ok(_tutorRep.TutorList);
            }
            catch
            {
                return BadRequest();
            }
        }
        */

        [HttpGet("{id}")]
        async public Task<ActionResult<List<Tutor>>> Get(string id)
        {
            try
            {
                await _tutorRep.GetAsync(id);
                return Ok(_tutorRep.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }


/*        [HttpDelete("{id}")]
        async public Task<ActionResult<List<Tutor>>> Delete(string id)
        {
            try
            {
                await _tutorRep.DeleteAsync(id);
                return Ok(_tutorRep.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }*/

        [HttpPut]
        async public Task<ActionResult<List<Tutor>>> Put(Tutor tutorUpdate)
        {
            try
            {
                await _tutorRep.UpdateAsync(tutorUpdate);
                return Ok(_tutorRep.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
