using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Enumerators;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlatRockTechnology.FamousQuoteQuiz.WebApi.Controllers
{
    public class AdminController : ControllerBase
    {
        IUserServices userServices;
        private readonly UserManager<User> _userManager;
        private readonly IAuthManager _authManager;
        private readonly ILogger<UserController> _logger;
        private readonly IQuestionService _questionService;
        public AdminController(IUserServices userServices,
                                UserManager<User> userManager,
                                ILogger<UserController> logger,
                                IAuthManager authManager,
                                IQuestionService questionService)
        {
            this.userServices = userServices;
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
            _questionService = questionService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] UserModel request)
        {
            request.Roles = new List<string>()
            {
                "Administrator"
            };
            _logger.LogInformation($"Registration Attempt for {request.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await userServices.Register(request);

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
                _logger.LogError(e, $"Something Went Wrong in the {nameof(AddAdmin)}");
                return Problem($"Something Went Wrong in the {nameof(AddAdmin)}", statusCode: 500);
            }
            return Ok();
        }


        // Admin methods about user
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("DisableOrEnableUser/{UserID}")]
        public async Task<UserModel> DisableOrEnableUser(string UserID)
        {
            return await userServices.DisableOrEnableUser(UserID);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("DeleteUser/{UserID}")]
        public async Task<IActionResult> DeleteUser(string UserID)
        {
            await userServices.DeleteUserAsync(UserID);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("UpdateUserEmail/{id}/{newEmail}")]
        public async Task<IActionResult> UpdateUserEmail(string id, string newEmail)
        {
            if(await userServices.UpdateEmailAsync(id, newEmail))
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("CreateQuestion/{Type}")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionInsertModel question, string Type)
        {
            var type = (QuizType)Enum.Parse(typeof(QuizType), Type, true);
            await _questionService.AddQuestion(type, question);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("UpdateQuote/{id}/{questionText}")]
        public async Task<IActionResult> UpdateQuote(long id, string questionText)
        {
            await _questionService.UpdateAsync(id, questionText);
            return Ok();
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("DeleteQuote/{QuoteID}")]
        public async Task<IActionResult> DeleteQuote(long QuoteID)
        {
            await _questionService.DeleteAsync(QuoteID);
            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GetQuestions")]
        public IEnumerable<QuestionModel> GetQuestions()
        {
            return _questionService.GetModels();
        }
    }
}
