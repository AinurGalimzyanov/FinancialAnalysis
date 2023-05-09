using Dal.User.Entity;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.User;

public class UserManager 
{
    private readonly SignInManager<UserDal> _signInManager;
    private readonly UserManager<UserDal> _userManager;

    public UserManager(SignInManager<UserDal> signInManager, UserManager<UserDal> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<string> RecoverPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var newPassword = Guid.NewGuid().ToString();
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, newPassword);
        return newPassword;
    }
}