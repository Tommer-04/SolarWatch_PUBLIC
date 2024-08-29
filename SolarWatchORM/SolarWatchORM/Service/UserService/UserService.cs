using Microsoft.AspNetCore.Identity;
using SolarWatchORM.Configurations;
using SolarWatchORM.Model;
using System.Data;

namespace SolarWatchORM.Service.UserService
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtTokenHelper _tokenHelper;

        public UserService(UserManager<IdentityUser> userManager, JwtTokenHelper tokenHelper)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model, string role)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<string?> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var valid = await _userManager.CheckPasswordAsync(user, model.Password);
                if (valid)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return _tokenHelper.GenerateToken(user, roles);
                }
            }
            return null;
        }
    }
}
