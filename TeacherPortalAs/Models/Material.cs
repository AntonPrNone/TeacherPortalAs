using Postgrest.Attributes;
using Postgrest.Models;

namespace TeacherPortalAs.Models;

[Table("material")]
public class Material : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("subject_id")]
    public int SubjectId { get; set; }

    [Column("created")]
    public DateTime Created { get; set; }

    [Column("updated")]
    public DateTime? Updated { get; set; }
} 