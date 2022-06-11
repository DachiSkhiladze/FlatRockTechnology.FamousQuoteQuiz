using Microsoft.AspNetCore.Identity;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database
{
    public class User : IdentityUser
    {
        public User()
        {
            UserAnsweredQuestion = new HashSet<UserAnsweredQuestion>();
        }
        public bool? IsDisabled { get; set; }
        public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestion { get; set; }
    }
}
