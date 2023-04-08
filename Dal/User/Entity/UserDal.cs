using System.ComponentModel.DataAnnotations;
using Dal.Categories.Entity;
using Dal.Operation.Entity;
using Microsoft.AspNetCore.Identity;

namespace Dal.User.Entity;

public class UserDal : IdentityUser
{
    public int? Balance { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; }

    public List<CategoriesDal>? CategoriesList { get; set; } 
    
    public List<OperationDal>? OperationList { get; set; }
}