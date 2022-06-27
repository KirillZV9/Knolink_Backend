using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Models;
using PomogatorAPI.Repositories;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ITutor _tutorService;
        private ClaimsIdentity? Identity { get; set; }
        private string Id { get { return AuthService.GetUserId(Identity); } }

        public OrderController (IOrder orderService, ITutor tutorService)
        {
            _orderService = orderService;
            _tutorService = tutorService;   
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        async public Task<ActionResult> Post(string customerId, string subject, string description, string type)
        {
            Identity = User.Identity as ClaimsIdentity;

            if (Id != customerId)
                return Forbid();

            try
            {
                await _orderService.PostAsync(customerId, subject, description, type);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        async public Task<ActionResult<Order>> Get(string id)
        {
            try
            {
                await _orderService.GetAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetByCustomerId")]
        [Authorize(Roles = "customer")]
        async public Task<ActionResult<Dictionary<string, Order>>> GetByCustomerId()
        {
            Identity = User.Identity as ClaimsIdentity;

            try
            {
                await _orderService.GetOrdersById(Id, "CustomerId");

                return Ok(_orderService.OrdersDict);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetByTutorId")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult<Dictionary<string, Order>>> GetByTutorId()
        {
            Identity = User.Identity as ClaimsIdentity;

            try
            {
                await _orderService.GetOrdersById(Id, "TutorId");

                return Ok(_orderService.OrdersDict);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetBySubject")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult<Dictionary<string, Order>>> GetBySubject(string subject)
        {
            try
            {
                await _orderService.GetOrdersBySubject(subject);

                return Ok(_orderService.OrdersDict);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("PostResponse")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult> PostResponse(string orderId)
        {
            Identity = User.Identity as ClaimsIdentity;

            try
            {
                await _orderService.PostResponse(Id, orderId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRespondedTutors")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<List<Tutor>>> GetRespondedTutors(string orderId)
        {
            try
            { 
                await _tutorService.GetTutorsById(await _orderService.GetAllRespondedTutors(orderId));

                return Ok(_tutorService.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch("CloseOrder")]
        [Authorize(Roles = "tutor")]
        public async Task<ActionResult> CloseOrder(string orderId)
        {
            try
            {
                await _orderService.CloseOrder(orderId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("RateTutor")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> RateTutor(string orderId, string )
    }

}
