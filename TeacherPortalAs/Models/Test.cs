using Postgrest.Attributes;
using Postgrest.Models;

namespace TeacherPortalAs.Models;

[Table("test")]
public class Test : BaseModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("subject_id")]
    public int SubjectId { get; set; }

    [Column("created")]
    public DateTime Created { get; set; }

    [Column("updated")]
    public DateTime? Updated { get; set; }
} 