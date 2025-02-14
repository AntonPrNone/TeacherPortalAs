using Postgrest.Models;
using Postgrest.Attributes;

[Table("blog_posts")]
public class BlogPost : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("is_published")]
    public bool IsPublished { get; set; } = true;
} 