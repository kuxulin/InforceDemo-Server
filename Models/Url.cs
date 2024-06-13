using System.Text.Json.Serialization;

namespace back.Models;

public class Url : BaseEntity
{
    public string LongVersion { get; set; }
    public string ShortVersion { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Code { get; set; }
}
