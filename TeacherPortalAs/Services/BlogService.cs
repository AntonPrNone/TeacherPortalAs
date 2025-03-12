using Supabase;
using TeacherPortalAs.Models;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace TeacherPortalAs.Services
{
    public interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPostsAsync(bool includeUnpublished = false);
        Task<BlogPost?> GetBlogPostAsync(int id);
        Task<BlogPost> CreateBlogPostAsync(BlogPost post);
        Task<BlogPost> UpdateBlogPostAsync(BlogPost post);
        Task DeleteBlogPostAsync(int id);
        Task<List<BlogPost>> SearchBlogPostsAsync(string query, bool searchInContent = false);
    }

    public class BlogService : IBlogService
    {
        private readonly Client _supabase;
        private List<BlogPost>? _cachedPosts;

        public BlogService(Client supabase)
        {
            _supabase = supabase;
        }

        private void InvalidateCache()
        {
            _cachedPosts = null;
        }

        public async Task<List<BlogPost>> GetBlogPostsAsync(bool includeUnpublished = false)
        {
            if (_cachedPosts != null)
                return includeUnpublished 
                    ? _cachedPosts 
                    : _cachedPosts.Where(x => x.IsPublished).ToList();

            var response = await _supabase.From<BlogPost>().Get();
            _cachedPosts = response.Models;
            
            return includeUnpublished 
                ? _cachedPosts 
                : _cachedPosts.Where(x => x.IsPublished).ToList();
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
            
            InvalidateCache();
            return response.Models.First();
        }

        public async Task<BlogPost> UpdateBlogPostAsync(BlogPost post)
        {
            var response = await _supabase
                .From<BlogPost>()
                .Where(x => x.Id == post.Id)
                .Update(post);
            
            InvalidateCache();
            return response.Models.First();
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            await _supabase
                .From<BlogPost>()
                .Where(x => x.Id == id)
                .Delete();
            
            InvalidateCache();
        }

        public async Task<List<BlogPost>> SearchBlogPostsAsync(string query, bool searchInContent = false)
        {
            var posts = await GetBlogPostsAsync();
            
            if (string.IsNullOrWhiteSpace(query))
                return posts;
        
            return posts.Where(post =>
                post.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (searchInContent && post.Content.Contains(query, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
} 