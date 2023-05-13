using Dal.Base.Repositories;
using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Dal.Operation.Entity;
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
            .Where(x => (x.UserDal.Id == userId || x.UserDal == null) && x.Type == type)
            .ToListAsync();
        return categories;
    }

    public async Task<int?> GetSumCategory(Guid catId)
    {
        return  await _context.Set<CategoriesDal>()
            .Where(x => x.Id == catId)
            .Include(x => x.OperationList)
            .SelectMany(x => x.OperationList)
            .Select(x => x.Price)
            .SumAsync();
    }
}