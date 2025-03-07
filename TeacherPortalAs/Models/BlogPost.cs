using Postgrest.Models;
using Postgrest.Attributes;
using System;

namespace TeacherPortalAs.Models
{
    [Table("blog_posts")]
    public class BlogPost : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("is_published")]
        public bool IsPublished { get; set; } = false;
    }
} 