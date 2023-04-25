using Dal.Base.Repositories;
using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Categories.Repositories;

public class CategoriesRepository : BaseRepository<CategoriesDal, Guid>, ICategoriesRepository
{
    private readonly DataContext _context;
    
    public CategoriesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public  List<CategoriesDal> GetAllUserCategory(string userId)
    {
        /*var user = _context.Set<UserDal>()
            .Include(x => x.CategoriesList)
            .FirstOrDefault(x => x.Id == userId);*/
        return _context.Set<CategoriesDal>().Where(x => x.UserDal.Id == userId).ToList();
    }
}