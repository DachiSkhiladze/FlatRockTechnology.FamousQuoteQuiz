using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserLoginModel userLoginModel);
        Task<string> CreateToken();
        Task<User> GetUser(ClaimsPrincipal user);
    }
}
