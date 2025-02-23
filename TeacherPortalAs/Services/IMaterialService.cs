using TeacherPortalAs.Models;

namespace TeacherPortalAs.Services;

public interface IMaterialService
{
    Task<List<Material>> GetMaterialsAsync();
    Task<Material?> GetMaterialByIdAsync(int id);
    Task<List<Material>> GetMaterialsBySubjectIdAsync(int subjectId);
    Task<Material> CreateMaterialAsync(Material material);
    Task<Material> UpdateMaterialAsync(Material material);
    Task DeleteMaterialAsync(int? id);
} 