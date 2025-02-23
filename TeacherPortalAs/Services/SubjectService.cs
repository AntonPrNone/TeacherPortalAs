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
        await EnsureAuthenticatedAsync();
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
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Subject>()
            .Insert(subject);
        
        return response.Models.FirstOrDefault();
    }

    public async Task<Subject> UpdateSubjectAsync(Subject subject)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Subject>()
            .Update(subject);
        
        return response.Models.FirstOrDefault();
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        await _supabaseClient
            .From<Subject>()
            .Where(x => x.Id == id)
            .Delete();
        
        return true;
    }
} 