using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlatRockTechnology.FamousQuoteQuiz.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserServices _userServices;
        private readonly IAuthManager _authManager;
        private readonly ILogger<UserController> _logger;
        private readonly IUserQuestionsHistoryService _userQuestionsHistoryService;
        private readonly IQuestionsHistoryService _questionsHistoryService;

        public UserController(IUserServices userServices,
                                IAuthManager authManager,
                                ILogger<UserController> logger,
                                IUserQuestionsHistoryService userQuestionsHistoryService,
                                IQuestionsHistoryService questionsHistoryService)
        {
            _userServices = userServices;
            _authManager = authManager;
            _logger = logger;
            _userQuestionsHistoryService = userQuestionsHistoryService;
            _questionsHistoryService = questionsHistoryService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GetUserHistory/{userID}")]
        public IEnumerable<UserQuestionHistory> GetUserHistory(string userID)
        {
            if (_questionsHistoryService.CheckIfExists(o => o.UserID == userID))
            {
                var result = _userQuestionsHistoryService.GetUserQuestionHistoryAsync(userID).ToEnumerable();
                return result;
            }
            return Enumerable.Empty<UserQuestionHistory>();
        }


        //[Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GetUsers")]
        public IEnumerable<UserModel> GetUsers()
        {
            return _userServices.GetModels();
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpGet]
        [Route("CheckIfUserAuthorized")]
        public IActionResult CheckIfUserAuthorized()
        {
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("CheckIfAdminAuthorized")]
        public IActionResult CheckIfAdminAuthorized()
        {
            return Ok();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserModel request)
        {
            request.Roles = new List<string>()
            {
                "User"
            };
            request.IsDisabled = false;
            _logger.LogInformation($"Registration Attempt for {request.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userServices.Register(request);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
            return Ok();
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _authManager.ValidateUser(request))
            {
                return Unauthorized();
            }

            return Accepted(new { Token = await _authManager.CreateToken() });
        }
    }
}
