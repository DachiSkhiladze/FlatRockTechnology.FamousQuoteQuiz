using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using Microsoft.AspNetCore.Identity;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations.UserQuestionsHistory
{
    public class UserQuestionsHistoryService : IUserQuestionsHistoryService
    {
        private readonly UserManager<User> _userManager;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IQuestionsHistoryService _questionsHistoryService;
        public UserQuestionsHistoryService(UserManager<User> userManager,
                                       IQuestionService questionService,
                                       IAnswerService answerService,
                                       IQuestionsHistoryService questionsHistoryService)
        {
            _userManager = userManager;
            _questionService = questionService;
            _answerService = answerService;
            _questionsHistoryService = questionsHistoryService;
        }

        public async IAsyncEnumerable<UserQuestionHistory> GetUserQuestionHistoryAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            //_questionsHistoryService
            IEnumerable<UserAnsweredQuestionModel> questionsAnswered = _questionsHistoryService.GetModels(o => o.UserID.Equals(user.Id));
            List<QuestionModel> questions = new List<QuestionModel>();
            foreach (var question in questionsAnswered)
            {
                var question1 = _questionService.GetModels(o => o.ID == question.QuestionID).FirstOrDefault();
                if (question1 != null)
                {
                    questions.Add(question1);
                }
            }
            foreach (var question in questions)
            {
                var userAnswer = questionsAnswered.First(o => o.QuestionID.Equals(question.ID));
                var answerModel = _answerService.GetModels(o => o.ID == userAnswer.AnswerID).FirstOrDefault();
                if (answerModel != null)
                {
                    yield return new UserQuestionHistory()
                    {
                        QuestionText = question.Text,
                        AnswerText = answerModel.Text,
                        WasCorrect = answerModel.IsCorrect,
                        Question = question,
                        Answers = _answerService.GetModels(o => o.QuestionID.Equals(question.ID)),
                        UserAnsweredQuestion = questionsAnswered.First(o => o.QuestionID.Equals(question.ID))
                    };
                }
            }
        }
    }
}
