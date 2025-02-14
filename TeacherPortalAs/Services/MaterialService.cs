using TeacherPortalAs.Models;

namespace TeacherPortalAs.Services;

public class MaterialService : IMaterialService
{
    private readonly Supabase.Client _supabaseClient;

    public MaterialService(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<List<Material>> GetMaterialsAsync()
    {
        var response = await _supabaseClient
            .From<Material>()
            .Get();
        return response.Models;
    }

    public async Task<Material?> GetMaterialByIdAsync(int id)
    {
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == id)
            .Get();
        return response.Models.FirstOrDefault();
    }

    public async Task<List<Material>> GetMaterialsBySubjectIdAsync(int subjectId)
    {
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.SubjectId == subjectId)
            .Get();
        return response.Models;
    }

    public async Task<Material> CreateMaterialAsync(Material material)
    {
        var response = await _supabaseClient
            .From<Material>()
            .Insert(material);
        return response.Models.First();
    }

    public async Task<Material> UpdateMaterialAsync(Material material)
    {
        var response = await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == material.Id)
            .Update(material);
        return response.Models.First();
    }

    public async Task DeleteMaterialAsync(int id)
    {
        await _supabaseClient
            .From<Material>()
            .Where(x => x.Id == id)
            .Delete();
    }
} 