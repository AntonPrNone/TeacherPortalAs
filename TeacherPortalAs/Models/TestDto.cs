using Postgrest.Attributes;
using Postgrest.Models;
using TeacherPortalAs.Models;

[Table("tests")]
public class TestDto : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [Column("tags")]
    public string Tags { get; set; } = string.Empty;

    [Column("is_published")]
    public bool IsPublished { get; set; }

    [Column("questions_json")]
    public string QuestionsJson { get; set; } = "[]";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public Test ToTest()
    {
        return new Test
        {
            Id = Id,
            Title = Title,
            Description = Description,
            ImageUrl = ImageUrl,
            Tags = Tags,
            IsPublished = IsPublished,
            QuestionsJson = QuestionsJson,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
} 