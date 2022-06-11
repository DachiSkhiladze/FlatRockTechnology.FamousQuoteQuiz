using AutoMapper;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations
{
    public class QuestionsHistoryService : BaseService<UserAnsweredQuestion, UserAnsweredQuestionModel>, IQuestionsHistoryService
    {

        protected IMapper Mapper;
        private readonly IAnswerService _answerService;
        public QuestionsHistoryService(IRepository<UserAnsweredQuestion> repository, 
                                       IMapper mapper,
                                       IAnswerService answerService) : base(repository)
        {
            this.Mapper = mapper;   // Initializing AutoMapper
            this._answerService = answerService;
        }

        public async Task<UserAnsweredQuestionModel> AddQuestion(string userId, long answerId)
        {
            var question = new UserAnsweredQuestionModel()
            {
                AnswerID = answerId,
                QuestionID = _answerService.GetModels(o => o.ID == answerId).First().QuestionID,
                UserID = userId
            };
            return await this.InsertAsync(question);
        }

        public bool CheckQuestion(string userId, long questionId)
        {
            var questions = this.GetModels(o => o.UserID.Equals(userId));
            return questions.Select(o => o.QuestionID).Contains(questionId);
        }

        protected override UserAnsweredQuestion ConvertToDTO(UserAnsweredQuestionModel model) => Mapper.Map<UserAnsweredQuestion>(model);

        protected override UserAnsweredQuestionModel ConvertToModel(UserAnsweredQuestion entity) => Mapper.Map<UserAnsweredQuestionModel>(entity);

        protected override IEnumerable<UserAnsweredQuestionModel> ConvertToModels(IQueryable<UserAnsweredQuestion> entities)
        {
            foreach (var item in entities)
            {
                yield return Mapper.Map<UserAnsweredQuestionModel>(item);
            }
        }
    }
}
