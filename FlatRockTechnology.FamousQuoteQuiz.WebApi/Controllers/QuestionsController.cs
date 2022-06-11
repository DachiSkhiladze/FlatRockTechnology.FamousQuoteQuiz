using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Enumerators;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlatRockTechnology.FamousQuoteQuiz.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionsHistoryService _questionsHistoryService;
        private readonly IAnswerService _answerService;
        public QuestionsController(IQuestionService questionService, 
                                    IQuestionsHistoryService questionsHistoryService,
                                    IAnswerService answerService)
        {
            _questionService = questionService;
            _questionsHistoryService = questionsHistoryService;
            _answerService = answerService;
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpGet]
        [Route("GetQuestion/{QuizType}")]
        public QuestionModel GetQuestion(string QuizType)
        {
            var userId = this.GetCurrentUserID();
            QuizType type = (QuizType)Enum.Parse(typeof(QuizType), QuizType, true);
            var question = _questionService.GetQuestion(type, userId);

            if(question == null)
            {
                question = new QuestionModel() { Text = "You Have Answered All The Questions In this Mode"};
            }
            return question;
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpGet]
        [Route("AnswerQuestion/{answerId}")]
        public async Task<AnswerModel> AnswerQuestion(long answerId)
        {
            var userId = this.GetCurrentUserID();
            await _questionsHistoryService.AddQuestion(userId, answerId);
            return _answerService.CheckTheAnswer(answerId);
        }

        private string GetCurrentUserID()
        {
            var id = (HttpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
            return id;
        }
    }
}