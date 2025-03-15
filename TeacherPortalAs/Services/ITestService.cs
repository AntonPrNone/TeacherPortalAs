using TeacherPortalAs.Models;

namespace TeacherPortalAs.Services
{
    public interface ITestService
    {
        Task<List<Test>> GetTestsAsync(bool includeUnpublished = false);
        Task<Test?> GetTestByIdAsync(int id);
        Task<List<Test>> SearchTestsAsync(string query, string? tag = null, int? gradeLevel = null);
        Task<Test> CreateTestAsync(Test test);
        Task<Test> UpdateTestAsync(Test test);
        Task<bool> DeleteTestAsync(int id);
        Task<List<string>> GetAllTagsAsync(); // Получение списка всех уникальных тегов
    }
} 