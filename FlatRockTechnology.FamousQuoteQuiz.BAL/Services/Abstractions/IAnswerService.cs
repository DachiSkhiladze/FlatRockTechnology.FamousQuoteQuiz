using FlatRockTechnology.FamousQuoteQuiz.BAL.Models;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IAnswerService : IBaseService<Answer, AnswerModel>
    {
        public AnswerModel CheckTheAnswer(long questionId);
    }
}
