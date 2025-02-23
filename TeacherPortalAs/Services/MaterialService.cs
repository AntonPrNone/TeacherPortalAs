using TeacherPortalAs.Models;
using Microsoft.JSInterop;

namespace TeacherPortalAs.Services;

public class MaterialService(Supabase.Client supabaseClient, IJSRuntime js) : IMaterialService
{
    private readonly Supabase.Client _supabaseClient = supabaseClient;
    private readonly IJSRuntime _js = js;
    private List<Material>? _cachedMaterials;

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
        if (_cachedMaterials != null)
            return _cachedMaterials;

        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Get();
        
        _cachedMaterials = response.Models;
        return _cachedMaterials;
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
        
        var createdMaterial = response.Models.First();
        _cachedMaterials?.Add(createdMaterial);
        return createdMaterial;
    }

    public async Task<Material> UpdateMaterialAsync(Material material)
    {
        await EnsureAuthenticatedAsync();
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == material.Id)
            .Update(material);
        
        if (_cachedMaterials != null)
        {
            var index = _cachedMaterials.FindIndex(x => x.Id == material.Id);
            if (index != -1)
            {
                _cachedMaterials[index] = response.Models.First();
            }
        }
        return response.Models.First();
    }

    public async Task DeleteMaterialAsync(int? id)
    {
        await EnsureAuthenticatedAsync();
        await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == id)
            .Delete();
        
        _cachedMaterials?.RemoveAll(x => x.Id == id);
    }
} 