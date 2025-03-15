using Supabase;
using TeacherPortalAs.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace TeacherPortalAs.Services
{
    public class TestService : ITestService
    {
        private readonly Supabase.Client _supabase;
        private readonly NavigationManager _navigationManager;
        private List<Test>? _cachedTests;

        public TestService(Supabase.Client supabase, NavigationManager navigationManager)
        {
            _supabase = supabase;
            _navigationManager = navigationManager;
        }

        private void InvalidateCache() => _cachedTests = null;

        private async Task EnsureAuthenticatedAsync()
        {
            var session = await _supabase.Auth.RetrieveSessionAsync();
            if (session == null)
            {
                _navigationManager.NavigateTo("/admin/login");
            }
        }

        public async Task<List<Test>> GetTestsAsync(bool includeUnpublished = false)
        {
            if (_cachedTests != null)
                return includeUnpublished 
                    ? _cachedTests 
                    : _cachedTests.Where(x => x.IsPublished).ToList();

            var response = await _supabase.From<Test>().Get();
            _cachedTests = response.Models;
            
            return includeUnpublished 
                ? _cachedTests 
                : _cachedTests.Where(x => x.IsPublished).ToList();
        }

        public async Task<Test?> GetTestByIdAsync(int id)
        {
            return await _supabase.From<Test>()
                .Where(x => x.Id == id)
                .Single();
        }

        public async Task<List<Test>> SearchTestsAsync(string query, string? tag = null, int? gradeLevel = null)
        {
            var tests = await GetTestsAsync();
            
            // Фильтруем по поисковому запросу
            if (!string.IsNullOrWhiteSpace(query))
            {
                tests = tests.Where(test =>
                    test.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    test.Description.Contains(query, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Фильтруем по тегу
            if (!string.IsNullOrWhiteSpace(tag))
            {
                tests = tests.Where(test => 
                    test.Tags.Split(',')
                        .Select(t => t.Trim())
                        .Contains(tag, StringComparer.OrdinalIgnoreCase)
                ).ToList();
            }
            
            return tests;
        }

        public async Task<Test> CreateTestAsync(Test test)
        {
            try
            {
                await EnsureAuthenticatedAsync();

                var response = await _supabase
                    .From<TestDto>()
                    .Insert(new[] { new TestDto
                    {
                        Title = test.Title,
                        Description = test.Description,
                        ImageUrl = test.ImageUrl,
                        Tags = test.Tags,
                        IsPublished = test.IsPublished,
                        QuestionsJson = test.QuestionsJson,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }});

                var createdTest = response.Models.First().ToTest();
                InvalidateCache();
                return createdTest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test: {ex.Message}");
                throw;
            }
        }

        public async Task<Test> UpdateTestAsync(Test test)
        {
            try
            {
                await EnsureAuthenticatedAsync();

                var response = await _supabase
                    .From<TestDto>()
                    .Upsert(new[] { new TestDto
                    {
                        Id = test.Id,
                        Title = test.Title,
                        Description = test.Description,
                        ImageUrl = test.ImageUrl,
                        Tags = test.Tags,
                        IsPublished = test.IsPublished,
                        QuestionsJson = test.QuestionsJson,
                        UpdatedAt = DateTime.Now
                    }});

                var updatedTest = response.Models.First().ToTest();
                InvalidateCache();
                return updatedTest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating test: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteTestAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            
            try
            {
                await _supabase.From<Test>()
                    .Where(x => x.Id == id)
                    .Delete();
                
                InvalidateCache();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting test: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetAllTagsAsync()
        {
            var tests = await GetTestsAsync();
            return tests
                .SelectMany(t => t.Tags.Split(','))
                .Select(tag => tag.Trim())
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct()
                .OrderBy(tag => tag)
                .ToList();
        }
    }
} 