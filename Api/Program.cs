using System.Text;
using Api;
using Api.Controllers.Operation.Mapping;
using Api.Controllers.Public.Auth.Mapping;
using Api.Controllers.Public.Categories.Mapping;
using AutoMapper;
using Dal;
using Dal.Categories.Repositories;
using Dal.Categories.Repositories.Interface;
using Dal.Email.Repositories;
using Dal.Email.Repositories.Interface;
using Dal.Operation.Repositories;
using Dal.Operation.Repositories.Interface;
using Dal.User.Entity;
using Dal.User.Repositories;
using Logic.Managers.Categories;
using Logic.Managers.Categories.Interface;
using Logic.Managers.Operation;
using Logic.Managers.Operation.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTSettings:Audience"],
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:SecretKey"]))
        };
    });
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// добавление айдентити, тестовая
// надо усложнить требования к паролю
builder.Services.AddIdentity<UserDal, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();
    

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<UserDal>()
    .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddDeveloperSigningCredential();
    
    
builder.Services.AddControllers();


// Тестовые репозиторий для бд почты. Требует удаления
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
// Репозиторий пользователя
builder.Services.AddScoped<UserRepository>();
// Мененджер пользователя
builder.Services.AddScoped<UserManager<UserDal>>();
// ???
//builder.Services.AddScoped(typeof(Logic.Managers.UserManager<>));
// Мененджер ролей из идентити
builder.Services.AddScoped<RoleManager<IdentityRole>>();

//репозитории и менеджер Категрий
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesManager, CategoriesManager>();
//репозитории и менеджер Операций
builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IOperationManager, OperationManager>();

// Маппинг 
builder.Services.AddAutoMapper(typeof(AccountMappingProfile));
builder.Services.AddAutoMapper(typeof(CreateCategoriesProfile));
builder.Services.AddAutoMapper(typeof(GetCategoryRequestProfile));
builder.Services.AddAutoMapper(typeof(UpdateCategoryProfile));
builder.Services.AddAutoMapper(typeof(CreateOperationProfile));
builder.Services.AddAutoMapper(typeof(UpdateOperationProfile));


builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    var xmlPath = Path.Combine(basePath, "Api.xml");
    options.IncludeXmlComments(xmlPath);
});*/
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseHttpsRedirection();

// Подключаем авторизацию, аутентификацию и айдентити
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.MapControllers();

app.Run();