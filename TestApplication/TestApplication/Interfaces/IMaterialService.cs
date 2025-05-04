using TestApplication.DTO;

namespace TestApplication.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialListResponseDto> GetAllAsync();
        Task<MaterialDto?> GetByIdAsync(int id);
        Task<MaterialListResponseDto> CreateAsync(CreateMaterialDto material);
        Task<MaterialListResponseDto> UpdateAsync(UpdateMaterialDto material);
        Task<MaterialListResponseDto> DeleteAsync(int id);
    }
}
