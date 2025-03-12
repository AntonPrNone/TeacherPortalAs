using TeacherPortalAs.Models;
using Supabase;
using Microsoft.JSInterop;
using System.Text.Json;
using Supabase.Gotrue;
using Microsoft.AspNetCore.Components;

namespace TeacherPortalAs.Services;

public class SubjectService : ISubjectService
{
    private readonly Supabase.Client _supabaseClient;
    private readonly NavigationManager _navigationManager;
    private List<Subject>? _cachedSubjects;

    private void InvalidateCache() => _cachedSubjects = null;

    public SubjectService(Supabase.Client supabaseClient, NavigationManager navigationManager)
    {
        _supabaseClient = supabaseClient;
        _navigationManager = navigationManager;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var session = await _supabaseClient.Auth.RetrieveSessionAsync();
        if (session == null)
        {
            _navigationManager.NavigateTo("/admin/login");
        }
    }

    public async Task<List<Subject>> GetSubjectsAsync()
    {
        if (_cachedSubjects != null)
            return _cachedSubjects;

        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Subject>()
            .Get();
        
        _cachedSubjects = response.Models;
        return _cachedSubjects;
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
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Subject>()
            .Insert(subject);
        
        _cachedSubjects?.Add(response.Models.FirstOrDefault());
        return response.Models.FirstOrDefault();
    }

    public async Task<Subject> UpdateSubjectAsync(Subject subject)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Subject>()
            .Update(subject);
        
        if (_cachedSubjects != null)
        {
            var index = _cachedSubjects.FindIndex(x => x.Id == subject.Id);
            if (index != -1)
            {
                _cachedSubjects[index] = response.Models.FirstOrDefault();
            }
        }
        return response.Models.FirstOrDefault();
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        
        try 
        {
            // Сначала удаляем все материалы, связанные с предметом
            await _supabaseClient
                .From<Material>()
                .Where(x => x.SubjectId == id)
                .Delete();
            
            // Затем удаляем сам предмет
            await _supabaseClient
                .From<Subject>()
                .Where(x => x.Id == id)
                .Delete();
            
            if (_cachedSubjects != null)
            {
                _cachedSubjects.RemoveAll(x => x.Id == id);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting subject: {ex.Message}");
            throw;
        }
    }
} 