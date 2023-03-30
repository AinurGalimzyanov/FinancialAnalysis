using System.ComponentModel.DataAnnotations;
using Dal.Base.Entity;

namespace Dal.Operation.Entity;

public class OperationDal : BaseDal<Guid>
{
    public string? Type;

    public int? Price;

    public DateTime DateTime;
}