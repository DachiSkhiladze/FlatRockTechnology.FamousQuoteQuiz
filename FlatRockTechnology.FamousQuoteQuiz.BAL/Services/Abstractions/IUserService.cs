using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using Microsoft.AspNetCore.Identity;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions
{
    public interface IUserServices : IBaseService<User, UserModel>
    {
        Task<IdentityResult> Register(UserModel model);
        Task<UserModel> DisableOrEnableUser(string id);
        Task DeleteUserAsync(string id);
        Task<bool> UpdateEmailAsync(string id, string newEmail);
    }
}
