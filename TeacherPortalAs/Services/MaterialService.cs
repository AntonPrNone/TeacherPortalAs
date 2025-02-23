using TeacherPortalAs.Models;
using Microsoft.JSInterop;

namespace TeacherPortalAs.Services;

public class MaterialService(Supabase.Client supabaseClient, IJSRuntime js) : IMaterialService
{
    private readonly Supabase.Client _supabaseClient = supabaseClient;
    private readonly IJSRuntime _js = js;

    private async Task EnsureAuthenticatedAsync()
    {
        var session = await _supabaseClient.Auth.RetrieveSessionAsync();
        if (session == null)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", ["supabase.auth.token"]);
            var refreshToken = await _js.InvokeAsync<string>("localStorage.getItem", ["supabase.auth.refresh_token"]);
            
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(refreshToken))
            {
                await _supabaseClient.Auth.SetSession(token, refreshToken);
            }
        }
    }

    public async Task<List<Material>> GetMaterialsAsync()
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Get();
        return response.Models;
    }

    public async Task<Material?> GetMaterialByIdAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == id)
            .Get();
        return response.Models.FirstOrDefault();
    }

    public async Task<List<Material>> GetMaterialsBySubjectIdAsync(int subjectId)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.SubjectId == subjectId)
            .Get();
        return response.Models;
    }

    public async Task<Material> CreateMaterialAsync(Material material)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Insert(material);
        return response.Models.First();
    }

    public async Task<Material> UpdateMaterialAsync(Material material)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == material.Id)
            .Update(material);
        return response.Models.First();
    }

    public async Task DeleteMaterialAsync(int id)
    {
        await EnsureAuthenticatedAsync();
        await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == id)
            .Delete();
    }
} 