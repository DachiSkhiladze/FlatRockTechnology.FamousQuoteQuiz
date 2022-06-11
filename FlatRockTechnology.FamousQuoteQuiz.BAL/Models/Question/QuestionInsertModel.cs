using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models
{
    public class QuestionInsertModel : QuestionModel
    {
        public IEnumerable<AnswerModel> Answers { get; set; }
    }
}
