using AutoMapper;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Answer;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<AnswerModel, Answer>();
            CreateMap<Answer, AnswerModel>();
            CreateMap<AnswerForUserModel, AnswerModel>();
            CreateMap<QuestionInsertModel, Question>();
            CreateMap<Question, QuestionInsertModel>();
            CreateMap<AnswerModel, AnswerForUserModel>();
            CreateMap<UserModel, User>();
            CreateMap<User, UserModel>();
            CreateMap<QuestionModel, Question>();
            CreateMap<Question, QuestionModel>();
            CreateMap<UserAnsweredQuestionModel, UserAnsweredQuestion>();
            CreateMap<UserAnsweredQuestion, UserAnsweredQuestionModel>();
        }
    }
}
