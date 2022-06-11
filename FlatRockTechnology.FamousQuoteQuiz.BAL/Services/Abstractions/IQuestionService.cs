using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Enumerators;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IQuestionService : IBaseService<Question, QuestionModel>
    {
        QuestionModel? GetQuestion(QuizType quizType, string userId);
        Task DeleteAsync(long id);
        Task UpdateAsync(long id, string text);
        Task AddQuestion(QuizType quizType, QuestionInsertModel question);
    }
}
