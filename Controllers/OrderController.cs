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
                return Ok(_orderService.Order);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetByCustomerId")]
        [Authorize(Roles = "customer")]
        async public Task<ActionResult<List<Order>>> GetByCustomerId()
        {
            try
            {
                await _orderService.GetOrdersById(Id, "CustomerId");

                return Ok(_orderService.OrdersList);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetByTutorId")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult<List<Order>>> GetByTutorId()
        {

            try
            {
                await _orderService.GetOrdersById(Id, "TutorId");

                return Ok(_orderService.OrdersList);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetBySubject")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult<List<Order>>> GetBySubject(string subject)
        {
            try
            {
                await _orderService.GetOrdersBySubject(subject);

                return Ok(_orderService.OrdersList);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult<List<Order>>> GetAll()
        {
            try
            {
                await _orderService.GetAllOrders();

                return Ok(_orderService.OrdersList);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("PostResponse")]
        [Authorize(Roles = "tutor")]
        async public Task<ActionResult> PostResponse(ResponseDTO dto)
        {

            try
            {
                await _orderService.PostResponse(dto.OrderId, Id, dto.Price);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRespondedTutors")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<List<RespondedTutor>>> GetRespondedTutors(string orderId)
        {
            try
            { 
                await _tutorService.GetTutorsById(await _orderService.GetAllRespondedTutors(orderId));

                return Ok(SetRespondedTutorsList(_orderService.RespondedTutors, _tutorService.Tutors));
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
        public async Task<ActionResult> RateTutor(RatingDTO dto)
        {
            try
            {
                await _orderService.PostTutorRating(dto.OrderId, dto.Rating);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch("SetTutor")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<Tutor>> SetTutor(string orderId, string tutorId)
        {
            try
            {
                await _orderService.SetTutor(orderId, tutorId);
                await _tutorService.GetAsync(tutorId);

                return Ok(_tutorService.Tutors);
            }
            catch
            {
                return BadRequest();
            }
        }

        private static List<RespondedTutor> SetRespondedTutorsList(Dictionary<string, string> responseDict, List<Tutor> tutorList)
        {
            List<RespondedTutor> respondedTutorList = new List<RespondedTutor>();

            foreach(Tutor tutor in tutorList)
            {
                var _respondedTutor = new RespondedTutor(tutor, responseDict[tutor.Id]);

                respondedTutorList.Add(_respondedTutor);
            }

            return respondedTutorList;
        }
    }

}
