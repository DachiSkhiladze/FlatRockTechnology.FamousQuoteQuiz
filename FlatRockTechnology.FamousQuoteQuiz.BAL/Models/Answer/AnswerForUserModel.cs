using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Answer
{
    public class AnswerForUserModel
    {
        public long ID { get; set; }
        public long QuestionID { get; set; }
        public string? Text { get; set; }
    }
}
