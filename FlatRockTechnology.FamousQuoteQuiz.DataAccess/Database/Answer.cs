using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database
{
    public class Answer
    {
        public Answer()
        {
            UserAnsweredQuestion = new HashSet<UserAnsweredQuestion>();
        }
        public long ID { get; set; }
        public long QuestionID { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }
        public string? Difficulty { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestion { get; set; }
    }
}
