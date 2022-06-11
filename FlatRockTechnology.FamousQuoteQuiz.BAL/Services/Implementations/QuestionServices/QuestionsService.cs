using AutoMapper;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Answer;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Enumerators;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations.QuestionServices
{
    public class QuestionService : BaseService<Question, QuestionModel>, IQuestionService
    {

        protected IMapper Mapper;
        private readonly IQuestionsHistoryService _questionsHistoryService;
        private readonly IAnswerService _answerService;
        public QuestionService(IRepository<Question> repository, IMapper mapper, IQuestionsHistoryService questionsHistoryService, IAnswerService answerService) : base(repository)
        {
            this.Mapper = mapper;   // Initializing AutoMapper Using InBuilt DI Service Container
            _questionsHistoryService = questionsHistoryService;
            _answerService = answerService;
        }

        private IEnumerable<QuestionModel> GetUnansweredQuestions(IEnumerable<QuestionModel> questionModels, string userId)
        {
            foreach (var question in _questionsHistoryService.GetModels(o => o.UserID == userId))
            {
                questionModels = questionModels.Where(o => o.ID != question.QuestionID);
            }
            return questionModels;
        }

        private QuestionModel? GetRandomQuestion(bool isBinary, string userId)
        {
            Random rand = new Random();
            var models = this.GetModels(o => o.IsBinary.Equals(isBinary));
            models = GetUnansweredQuestions(models, userId);
            int toSkip = rand.Next(0, (int)models.Count());
            return models.Skip(toSkip).FirstOrDefault();
        }

        public async Task DeleteAsync(long id)
        {
            var question = this.GetModels(o => o.ID == id).FirstOrDefault();
            if (question != null)
            {
                await this.DeleteAsync(question);
            }
        }

        public async Task UpdateAsync(long id, string text)
        {
            var question = this.GetModels(o => o.ID == id).FirstOrDefault();
            question.Text = text;
            if (question != null)
            {
                await this.UpdateAsync(question);
            }
        }

        public async Task AddQuestion(QuizType quizType, QuestionInsertModel question)
        {
            question.IsBinary = false;
            if (quizType == QuizType.BinaryQuestions)
            {
                question.IsBinary = true;
            }
            var questionInserted = await this.InsertAsync(question);
            foreach (var item in question.Answers)
            {
                item.QuestionID = questionInserted.ID;
                await _answerService.InsertAsync(item);
            }
        }

        // MultipleChoiceQuestions or BinaryQuestions
        // Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyMTJAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNlMDNlZmE2LTkzNjYtNGU4ZS05NzQ3LWEyN2E2NmIwOTY4NSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE2NTU5OTU1NzksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjgifQ.j8aIgXqw6ZrXGMWn9Pkvw7_2MqB6W0LmNEuNmBybb7Y
        public QuestionModel? GetQuestion(QuizType quizType, string userId)
        {
            bool isBinary = false;
            if (quizType == QuizType.BinaryQuestions)
            {
                isBinary = true;
            }

            var question = this.GetRandomQuestion(isBinary, userId);

            if(question == null)
            {
                return question;
            }

            //question = _questionsHistoryService.CheckQuestion(userId, question.ID) ? GetQuestion(quizType, userId) : question; // This line checks if the question is already answered by user. If it is, function does recursion and gets new question by random
            question.Answers = ConvertModelsForUserAnswers(_answerService.GetModels(o => o.QuestionID == question.ID));
            return question;
        }

        protected override Question ConvertToDTO(QuestionModel model) => Mapper.Map<Question>(model);

        protected override QuestionModel ConvertToModel(Question entity) => Mapper.Map<QuestionModel>(entity);

        protected override IEnumerable<QuestionModel> ConvertToModels(IQueryable<Question> entities)
        {
            foreach (var item in entities)
            {
                yield return Mapper.Map<QuestionModel>(item);
            }
        }

        protected IEnumerable<AnswerForUserModel> ConvertModelsForUserAnswers(IEnumerable<AnswerModel> entities)
        {
            foreach (var item in entities)
            {
                yield return Mapper.Map<AnswerForUserModel>(item);
            }
        }
    }
}
