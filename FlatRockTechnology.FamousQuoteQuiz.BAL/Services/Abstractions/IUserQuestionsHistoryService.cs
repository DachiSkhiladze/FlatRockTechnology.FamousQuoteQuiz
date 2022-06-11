using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IUserQuestionsHistoryService
    {
        IAsyncEnumerable<UserQuestionHistory> GetUserQuestionHistoryAsync(string userId);
    }
}
