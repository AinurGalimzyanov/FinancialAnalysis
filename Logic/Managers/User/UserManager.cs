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
    
    
    
}