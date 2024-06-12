namespace back.Models;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }
}
