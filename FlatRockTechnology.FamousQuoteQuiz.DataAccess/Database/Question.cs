using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database
{
    public class Question
    {
        public Question()
        {
            Answer = new HashSet<Answer>();
            UserAnsweredQuestion = new HashSet<UserAnsweredQuestion>();
        }

        public long ID { get; set; }
        public string? Text { get; set; }
        public string? AuthorName { get; set; }
        public bool? IsBinary { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestion { get; set; }
    }
}
