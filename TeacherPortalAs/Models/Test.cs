using Postgrest.Attributes;
using Postgrest.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TeacherPortalAs.Models;

[Table("tests")]
public class Test : BaseModel
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
    public string Tags { get; set; } = string.Empty; // Теги через запятую: "математика,алгебра,7 класс"

    [Column("is_published")]
    public bool IsPublished { get; set; } = false;

    [Column("questions_json")]
    public string QuestionsJson { get; set; } = "[]";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public List<TestQuestion> Questions { get; set; } = new List<TestQuestion>();

    public Test()
    {
        QuestionsJson = "[]";
        Questions = new List<TestQuestion>();
    }
}

public class TestQuestion
{
    public string Question { get; set; } = string.Empty;
    public List<TestAnswer> Answers { get; set; } = new();
}

public class TestAnswer
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
} 