using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IQuestionsHistoryService : IBaseService<UserAnsweredQuestion, UserAnsweredQuestionModel>
    {
        /*IAsyncEnumerable<UserQuestionHistory> GetUserQuestionHistoryAsync(string userId);*/
        bool CheckQuestion(string userId, long questionId);
        Task<UserAnsweredQuestionModel> AddQuestion(string userId, long answerId);
    }
}
