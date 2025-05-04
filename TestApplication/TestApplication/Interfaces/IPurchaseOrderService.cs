using TestApplication.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApplication.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrderHeaderDto>> GetAllOrdersAsync();
        Task<PurchaseOrderResponseDto> GetOrderByIdAsync(int id);
        Task<bool> DeleteOrderAsync(int id);
        Task<List<PurchaseOrderHeaderDto>> CreateOrderHeaderAsync(PurchaseOrderHeaderCreateDto header);
        Task<PurchaseOrderResponseDto> AddOrderDetailsAsync(int orderId, List<PurchaseOrderDetailDto> details);
        Task<bool> UpdateOrderAsync(int id, PurchaseOrderHeaderCreateDto header, List<PurchaseOrderDetailDto> details);
    }
}
