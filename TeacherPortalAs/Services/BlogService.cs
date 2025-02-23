using Supabase;
using TeacherPortalAs.Models;

namespace TeacherPortalAs.Services
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPostsAsync(bool includeUnpublished = false);
        Task<BlogPost?> GetBlogPostAsync(int id);
        Task<BlogPost> CreateBlogPostAsync(BlogPost post);
        Task<BlogPost> UpdateBlogPostAsync(BlogPost post);
        Task DeleteBlogPostAsync(int id);
    }

    public class BlogService : IBlogService
    {
        private readonly Client _supabase;
        private const string TABLE_NAME = "blog_posts";

        public BlogService(Client supabase) => _supabase = supabase;

        public async Task<List<BlogPost>> GetBlogPostsAsync(bool includeUnpublished = false)
        {
            var query = _supabase.From<BlogPost>();
            
            if (!includeUnpublished)
            {
                return (await query.Get()).Models.Where(x => x.IsPublished).ToList();
            }
            
            return (await query.Get()).Models;
        }

        public async Task<BlogPost?> GetBlogPostAsync(int id)
        {
            return await _supabase
                .From<BlogPost>()
                .Where(x => x.Id == id)
                .Single();
        }

        public async Task<BlogPost> CreateBlogPostAsync(BlogPost post)
        {
            post.Id = 0;
            var response = await _supabase
                .From<BlogPost>()
                .Insert(post);
            
            return response.Models.First();
        }

        public async Task<BlogPost> UpdateBlogPostAsync(BlogPost post)
        {
            var response = await _supabase
                .From<BlogPost>()
                .Where(x => x.Id == post.Id)
                .Update(post);
            
            return response.Models.First();
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            await _supabase
                .From<BlogPost>()
                .Where(x => x.Id == id)
                .Delete();
        }
    }
} 