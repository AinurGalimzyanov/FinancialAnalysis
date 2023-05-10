using Dal.Base.Repositories;
using Dal.Operation.Entity;
using Dal.Operation.Repositories.Interface;
using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Operation.Repositories;

public class OperationRepository : BaseRepository<OperationDal, Guid>, IOperationRepository
{
    private readonly DataContext _context;
    public OperationRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    private bool EqualsDate(string dateNow, string dateOperation)
    {
        return dateNow.Take(1).ToString() == dateOperation.Take(1).ToString();
    }


    public async Task<List<OperationDal>> GetAllOperationByCategoryAsync(string userId, Guid categoryId, DateTime date)
    {
        var operations = await _context
            .Set<OperationDal>()
            .Where(x => x.UserDal.Id == userId && x.CategoriesDal.Id == categoryId)
            .Where(x => x.DateTime.Value.Year == date.Year &&
                        x.DateTime.Value.Month == date.Month)
            .ToListAsync();
        return operations;
    }

    public async Task<List<OperationDal>> GetAllOperationByTypeAsync(string userId, string type, DateTime date)
    {
        var operations = await _context
            .Set<OperationDal>()
            .Where(x => x.UserDal.Id == userId && x.CategoriesDal.Type == type )
            .Where(x => x.DateTime.Value.Year == date.Year &&
                        x.DateTime.Value.Month == date.Month)
            .ToListAsync();
        return operations;
    }

    public async Task<List<OperationDal>> GetAllOperationsAsync(string userId, DateTime date)
    {
        var operations = await _context
            .Set<OperationDal>()
            .Where(x => x.UserDal.Id == userId)
            .Where(x => x.DateTime.Value.Year == date.Year &&
                        x.DateTime.Value.Month == date.Month)
            .ToListAsync();
        return operations;
    }

    public async Task<string> GetNameCategoryAsync(Guid operationId)
    {
        var dal = await _context
            .Set<OperationDal>()
            .Include(x => x.CategoriesDal)
            .FirstOrDefaultAsync(x => x.Id == operationId);

        return dal.CategoriesDal.Name;

    }
}

