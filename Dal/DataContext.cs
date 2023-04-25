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
        : base(options) { }
    
    
    
    /*using (var db = new DataContext())
    {
        var company1 = new CategoriesDal() { Name = "Работа" };
        var company2 = new CategoriesDal() { Name = "Аптека" };
        var company3 = new CategoriesDal() { Name = "Продукты" };
        var company4 = new CategoriesDal() { Name = "Подарки" };
        var company5 = new CategoriesDal() { Name = "Развлечение" };
 
        db.Companies.AddRange(company1, company2, company3, company4, company5); 
        db.SaveChanges();
    }*/
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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

        builder.Entity<UserDal>().Property(u => u.Balance).IsRequired(false);
        builder.Entity<CategoriesDal>().Property(u => u.Name).IsRequired(false);
        builder.Entity<OperationDal>().Property(u => u.DateTime);
        builder.Entity<OperationDal>().Property(u => u.Price).IsRequired(false);
        builder.Entity<OperationDal>().Property(u => u.Type).IsRequired(false);
    }
}