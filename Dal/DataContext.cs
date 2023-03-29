using Dal.User.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dal;

public class DataContext : IdentityDbContext<UserDal>
{
    
}