using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;

namespace Dal.Categories.Repositories.Interface;

public interface ICategoriesRepository : IBaseRepository<CategoriesDal, Guid>
{
    
}