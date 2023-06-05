using Dal.Base.Repositories;
using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Dal.Operation.Entity;
using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;

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
            .Where(x => (x.UserDal.Id == userId) && x.Type == type)
            .ToListAsync();
        return categories;
    }

    public async Task<int?> GetSumCategory(Guid catId, string userId)
    {
        return  await _context.Set<CategoriesDal>()
            .Where(x => x.Id == catId)
            .Include(x => x.OperationList)
            .SelectMany(x => x.OperationList)
            .Where(x => x.UserDal.Id == userId)
            .Select(x => x.Price)
            .SumAsync();
    }
    
    public async Task<List<Tuple<CategoriesDal, List<OperationDal>>>> GetCategoryWithOperation(string userId, DateTime from, DateTime to)
    {
        var c = await _context.Set<CategoriesDal>()
            .Where(x => x.UserDal.Id == userId)
            .Include(x => x.OperationList
                .Where(y => from <= y.DateTime.Value && y.DateTime.Value >= to))
            .Select(x => new Tuple<CategoriesDal, List<OperationDal>>(x, x.OperationList))
            .ToListAsync();

        return c;
    }

    public async Task<List<OperationDal>> GetOperations(Guid id)
    {
        return await _context.Set<CategoriesDal>()
            .Include(x => x.OperationList)
            .SelectMany(x => x.OperationList)
            .Where(x => x.CategoriesDal.Id == id)
            .ToListAsync();
    }
}