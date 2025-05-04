using TestApplication.DTO;

namespace TestApplication.Interfaces
{
    public interface IVendorService
    {
        Task<VendorListResponseDto> GetAllAsync();
        Task<VendorDto> GetByIdAsync(int id);
        Task<VendorListResponseDto> CreateAsync(CreateVendorDto vendor);
        Task<VendorListResponseDto> UpdateAsync(UpdateVendorDto vendor);
        Task<VendorListResponseDto> DeleteAsync(int id);
    }

}
