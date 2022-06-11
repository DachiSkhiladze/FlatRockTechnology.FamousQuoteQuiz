using AutoMapper;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations
{
    public class AnswerService : BaseService<Answer, AnswerModel>, IAnswerService
    {

        protected IMapper Mapper;
        public AnswerService(IRepository<Answer> repository, IMapper mapper) : base(repository)
        {
            this.Mapper = mapper;
        }

        public AnswerModel CheckTheAnswer(long answerId)
        {
            var answer = this.GetModels(o => o.ID == answerId).First();
            if (answer.IsCorrect)
            {
                return answer;
            }
            else
            {
                return this.GetModels(o => o.QuestionID == answer.QuestionID).First(o => o.IsCorrect);
            }
        }

        protected override Answer ConvertToDTO(AnswerModel model) => Mapper.Map<Answer>(model);

        protected override AnswerModel ConvertToModel(Answer entity) => Mapper.Map<AnswerModel>(entity);

        protected override IEnumerable<AnswerModel> ConvertToModels(IQueryable<Answer> entities)
        {
            foreach (var item in entities)
            {
                yield return Mapper.Map<AnswerModel>(item);
            }
        }
    }
}
