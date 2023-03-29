using Dal.Base.Entity;
using Dal.Base.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Dal.Base.Repositories;

public class BaseRepository<T, TI> : IBaseRepository<T, TI> where T : BaseDal<TI>
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dbSet;
    
    public Task<TI> InsertAsync(T dal)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(TI id)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetAsync(TI id)
    {
        throw new NotImplementedException();
    }

    public Task<TI> UpdateAsync(T dal)
    {
        throw new NotImplementedException();
    }
}