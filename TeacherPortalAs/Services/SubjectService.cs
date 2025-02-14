using TeacherPortalAs.Models;
using Supabase;

namespace TeacherPortalAs.Services;

public class SubjectService : ISubjectService
{
    private readonly Client _supabaseClient;

    public SubjectService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<List<Subject>> GetSubjectsAsync()
    {
        var response = await _supabaseClient
            .From<Subject>()
            .Get();
        
        return response.Models;
    }

    public async Task<Subject> GetSubjectByIdAsync(int id)
    {
        var response = await _supabaseClient
            .From<Subject>()
            .Where(x => x.Id == id)
            .Single();
        
        return response;
    }

    public async Task<Subject> CreateSubjectAsync(Subject subject)
    {
        var response = await _supabaseClient
            .From<Subject>()
            .Insert(subject);
        
        return response.Models.FirstOrDefault();
    }

    public async Task<Subject> UpdateSubjectAsync(Subject subject)
    {
        var response = await _supabaseClient
            .From<Subject>()
            .Update(subject);
        
        return response.Models.FirstOrDefault();
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        await _supabaseClient
            .From<Subject>()
            .Where(x => x.Id == id)
            .Delete();
        
        return true;
    }
} 