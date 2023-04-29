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
    
    public  List<OperationDal> GetAllUserCategoryOperationByType(string userId, Guid categoryId, string type)
    {
        return _context.Set<OperationDal>()
            .Where(x => x.UserDal.Id == userId && x.CategoriesDal.Id == categoryId && x.CategoriesDal.Type == type)
            .ToList();
    }
}

