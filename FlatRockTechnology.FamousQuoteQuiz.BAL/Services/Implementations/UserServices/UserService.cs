using AutoMapper;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.User;
using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations
{
    public class UserServices : BaseService<User, UserModel>, IUserServices
    {
        protected IMapper Mapper;
        private readonly UserManager<User> _userManager;
        public UserServices(IRepository<User> repository, IMapper mapper, UserManager<User> userManager) : base(repository)
        {
            this.Mapper = mapper;   // Initializing AutoMapper Using InBuilt DI Service Container
            _userManager = userManager;
        }

        public async Task<UserModel> DisableOrEnableUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsDisabled = !user.IsDisabled;
            await _userManager.UpdateAsync(user);
            return ConvertToModel(user);
        } 

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsDisabled = !user.IsDisabled;
            await _userManager.DeleteAsync(user);
        }

        public async Task<bool> UpdateEmailAsync(string id, string newEmail)
        {
            var test = await _userManager.FindByEmailAsync(newEmail);
            if (test!=null)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(id);
            user.Email = newEmail;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<IdentityResult> Register(UserModel model)
        {
            var user = ConvertToDTO(model);
            user.UserName = model.Email;
            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, model.Roles);
            }
            return result;
        }

        //Protected Methods
        protected override User ConvertToDTO(UserModel model) => Mapper.Map<User>(model);

        protected override UserModel ConvertToModel(User entity) => Mapper.Map<UserModel>(entity);

        protected override IEnumerable<UserModel> ConvertToModels(IQueryable<User> entities)
        {
            throw new NotImplementedException();
        }
    }
}
