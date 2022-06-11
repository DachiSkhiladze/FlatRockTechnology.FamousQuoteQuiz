using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database
{
    public class UserAnsweredQuestion
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public long QuestionID { get; set; }
        public long AnswerID { get; set; }
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
