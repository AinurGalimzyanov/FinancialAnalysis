using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;

namespace Dal.Categories.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    public Task<Guid> InsertAsync(CategoriesDal dal)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<CategoriesDal?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> UpdateAsync(CategoriesDal dal)
    {
        throw new NotImplementedException();
    }
}