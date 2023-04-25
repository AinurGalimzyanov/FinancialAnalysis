using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.User.Entity;

namespace Dal.Categories.Repositories.Interface;

public interface ICategoriesRepository : IBaseRepository<CategoriesDal, Guid>
{
    public List<CategoriesDal> GetAllUserCategory(string userId);

}