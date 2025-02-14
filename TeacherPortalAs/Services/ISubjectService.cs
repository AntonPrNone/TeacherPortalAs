using TeacherPortalAs.Models;

namespace TeacherPortalAs.Services;

public interface ISubjectService
{
    Task<List<Subject>> GetSubjectsAsync();
    Task<Subject> GetSubjectByIdAsync(int id);
    Task<Subject> CreateSubjectAsync(Subject subject);
    Task<Subject> UpdateSubjectAsync(Subject subject);
    Task<bool> DeleteSubjectAsync(int id);
} 