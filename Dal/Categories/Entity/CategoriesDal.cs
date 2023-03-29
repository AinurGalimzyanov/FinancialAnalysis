using System.ComponentModel.DataAnnotations;
using Dal.Base.Entity;

namespace Dal.Categories.Entity;

public class CategoriesDal : BaseDal<Guid>
{
    [MaxLength(255)] public string? Name;
}