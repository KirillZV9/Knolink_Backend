using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Models;
using System.Security.Claims;

namespace PomogatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly ICustomer _customerService;
        private readonly ITutor _tutorService;

        public AuthController(IAuth authService, ICustomer customerService, ITutor tutorService)
        {
            _authService = authService;
            _customerService = customerService;
            _tutorService = tutorService;

        }
        
        [HttpGet("getauthcode")]
        async public Task<ActionResult<string>> GetAuthCode(string id, string telNum)
        {
            try
            {
                await _authService.GetAuthCodeAsync(id, telNum);
                return Ok(_authService.AuthCode);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("codeverification")]
        async public Task<ActionResult<Dictionary<string, string>>> CodeVerification(string id, string authCode)
        {
            try
            {
                await _authService.CodeVerification(id, authCode);
                return Ok(new Dictionary<string, string>
                {
                    {"Id", _authService.User.Id},
                    {"TelNum", _authService.User.TelNum}
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("customersignup")]
        async public Task<ActionResult<string>> CustomerSignUp(CustomerDTO dto)
        {
            try
            {
                await _authService.SignIn(dto.Id, dto.AuthCode, "customer");

                await _customerService.PostAsync(dto.Id, dto.TelNum, dto.Name);

                return Ok(_authService.Token);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("customersignin")]
        async public Task<ActionResult<string>> CustomerSignIn(string id, string authCode)
        {
            try
            {
                if(_customerService.DoesCustomerExistAsync(id).Result)
                {
                  await  _authService.SignIn(id, authCode, "customer");
                  return Ok(_authService.Token);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("tutorsignup")]
        async public Task<ActionResult<string>> TutorSignUp(TutorDTO dto)
        {
            try
            {
                await _authService.SignIn(dto.Id, dto.AuthCode, "tutor");

                await _tutorService.PostAsync(dto.Id, dto.TelNum, dto.Name, dto.University);

                return Ok(_authService.Token);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("tutorsignin")]
        async public Task<ActionResult<string>> TutorSignIn(string id, string authCode)
        {
            try
            {
                if (_tutorService.DoesTutorExistAsync(id).Result)
                {
                    await _authService.SignIn(id, authCode, "tutor");
                    return Ok(_authService.Token);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
