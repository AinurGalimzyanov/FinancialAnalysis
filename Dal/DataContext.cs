using Dal.Categories.Entity;
using Dal.Email.Entity;
using Dal.Operation.Entity;
using Dal.User.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class DataContext : IdentityDbContext<UserDal>
{
    public DbSet<EmailDal> Email { get; set; }
    public DbSet<OperationDal> Operation { get; set; }
    public DbSet<CategoriesDal> Categories { get; set; }    

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CategoriesDal>().HasData(
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Продукты", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Развлечение", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Еда вне дома", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Транспорт", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Образование", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Спорт", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Подарки", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Здоровье", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Покупки", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "ЖКХ", Type = "expenses"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Связь", Type = "expenses"},
            
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Аванс", Type = "income"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Зарплата", Type = "income"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Больничные", Type = "income"},
            new CategoriesDal(){Id = Guid.NewGuid(), Name = "Премния", Type = "income"}
        );
        builder.Entity<UserDal>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity =>
            entity.ToTable(name: "UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity =>
            entity.ToTable(name: "UserClaim"));
        builder.Entity<IdentityUserLogin<string>>(entity =>
            entity.ToTable("UserLogins"));
        builder.Entity<IdentityUserToken<string>>(entity =>
            entity.ToTable("UserTokens"));
        builder.Entity<IdentityRoleClaim<string>>(entity =>
            entity.ToTable("RoleClaims"));

        builder.ApplyConfiguration(new AuthConfiguration());
        
        builder.Entity<EmailDal>().HasIndex(u => u.Email).IsUnique();
        /*builder.Entity<UserDal>().HasMany<CategoriesDal>().WithOne(x => x.UserDal).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<UserDal>().HasMany<OperationDal>().WithOne(x => x.UserDal).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<CategoriesDal>().HasMany<OperationDal>().WithOne(x => x.CategoriesDal).OnDelete(DeleteBehavior.Cascade);*/
        builder.Entity<UserDal>().Property(u => u.Balance).IsRequired(false);
        builder.Entity<CategoriesDal>().Property(u => u.Name).IsRequired(false);
        builder.Entity<OperationDal>().Property(u => u.DateTime);
        builder.Entity<OperationDal>().Property(u => u.Price).IsRequired(false);
        builder.Entity<CategoriesDal>().Property(u => u.Type).IsRequired(false);
    }
}