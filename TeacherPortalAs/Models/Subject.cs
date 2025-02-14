using Postgrest.Models;
using Postgrest.Attributes;

namespace TeacherPortalAs.Models;

[Table("subject")]
public class Subject : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [Column("icon_class")]
    public string IconClass { get; set; } = string.Empty;
    
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Column("updated")]
    public DateTime? Updated { get; set; }
} 