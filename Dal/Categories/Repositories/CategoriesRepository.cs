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

    public async Task<List<CategoriesDal>> GetAllUserCategory(string userId, string type)
    {
        var categories = await _context
            .Set<CategoriesDal>()
            .Where(x => x.UserDal.Id == userId && x.Type == type)
            .ToListAsync();
        return categories;
    }
}