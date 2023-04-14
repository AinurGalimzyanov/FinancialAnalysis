using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Dal.Categories.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly DataContext _context;

    public CategoriesRepository(DataContext context)
    {
        _context = context;
    }
    
    public  Task<Guid> InsertAsync(CategoriesDal dal)
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