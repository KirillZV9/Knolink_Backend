using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Models;
using PomogatorAPI.Repositories;
using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly ITutor _tutorService;
        private string Id { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public OrderController (IOrder orderService, ITutor tutorService)
        {
            _orderService = orderService;
            _tutorService = tutorService;   
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        async public Task<ActionResult> Post(OrderDTO dto)
        {
            
            if (Id != dto.CustomerId)
                return Forbid();

            try
            {
                await _orderService.PostAsync(dto.CustomerId, dto.Subject, dto.Description, dto.Type);
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
        async public Task<ActionResult> PostResponse(string orderId, string price)
        {

            try
            {
                await _orderService.PostResponse(Id, orderId, price);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRespondedTutors")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<Dictionary<Tutor, string>>> GetRespondedTutors(string orderId)
        {
            try
            { 
                await _tutorService.GetTutorsById(await _orderService.GetAllRespondedTutors(orderId));

                return Ok(SetTutorPriceDict(_orderService.RespondedTutors, _tutorService.Tutors));
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
        public async Task<ActionResult> RateTutor(string orderId, int rating)
        {
            try
            {
                await _orderService.PostTutorRating(orderId, rating);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        private static Dictionary<Tutor, string> SetTutorPriceDict(Dictionary<string, string> responseDict, List<Tutor> tutorList)
        {
            Dictionary<Tutor, string> tutorPriceDict = new Dictionary<Tutor, string>();

            foreach(Tutor tutor in tutorList)
            {
                tutorPriceDict.Add(tutor, responseDict[tutor.Id]);
            }

            return tutorPriceDict;
        }
    }

}
