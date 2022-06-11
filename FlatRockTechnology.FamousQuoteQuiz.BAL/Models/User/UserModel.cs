using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User
{
    public class UserModel : UserLoginModel
    {
        public string? ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsDisabled { get; set; }
        public ICollection<string>? Roles { get; set; }
    }
}
