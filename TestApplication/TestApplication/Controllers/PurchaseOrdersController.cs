using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderService _service;

    public PurchaseOrderController(IPurchaseOrderService service)
    {
        _service = service;
    }

    // Get All Orders
    [HttpGet]
    public async Task<ActionResult<PurchaseOrderListResponseDto>> GetAll()
    {
        var result = await _service.GetAllOrdersAsync();
        var response = new PurchaseOrderListResponseDto
        {
            Orders = result
        };
        return Ok(response); // Return a list of orders
    }

    // Get Order by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseOrderResponseDto>> GetById(int id)
    {
        var result = await _service.GetOrderByIdAsync(id);
        if (result == null)
        {
            return NotFound(new ErrorResponseDto { Message = "Purchase Order not found" });
        }

        var response = new PurchaseOrderResponseDto
        {
            Header = result.Header,
            Details = result.Details
        };
        return Ok(response); // Return specific order details
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PurchaseOrderListResponseDto>> Update(int id, PurchaseOrderCreateDto updateDto)
    {
        // Map Header (PurchaseOrderHeaderDto → PurchaseOrderHeaderCreateDto)
        var headerCreateDto = new PurchaseOrderHeaderCreateDto
        {
            OrderNumber = updateDto.Header.OrderNumber,
            OrderDate = updateDto.Header.OrderDate,
            VendorId = updateDto.Header.VendorId,
            Notes = updateDto.Header.Notes,
            OrderValue = updateDto.Header.OrderValue,
            IsActive = updateDto.Header.IsActive
        };

        var success = await _service.UpdateOrderAsync(id, headerCreateDto, updateDto.Details);

        if (!success)
        {
            return NotFound(new ErrorResponseDto { Message = "Purchase Order not found" });
        }

        // Return updated list of orders
        var orders = await _service.GetAllOrdersAsync();
        var response = new PurchaseOrderListResponseDto
        {
            Orders = orders
        };
        return Ok(response);
    }


    // Delete Order and Return Updated List
    [HttpDelete("{id}")]
    public async Task<ActionResult<PurchaseOrderListResponseDto>> Delete(int id)
    {
        var success = await _service.DeleteOrderAsync(id);
        if (success == null)
        {
            return NotFound(new ErrorResponseDto { Message = "Purchase Order not found" });
        }

        // Return updated list of orders after the delete
        var orders = await _service.GetAllOrdersAsync();
        var response = new PurchaseOrderListResponseDto
        {
            Orders = orders
        };
        return Ok(response); // Return updated list after delete
    }

    // Create Header and Return Updated List
    [HttpPost("create-header")]
    public async Task<ActionResult<PurchaseOrderListResponseDto>> CreateHeader(PurchaseOrderHeaderCreateDto header)
    {
        var updatedList = await _service.CreateOrderHeaderAsync(header);

        if (updatedList == null)
            return BadRequest(new ErrorResponseDto { Message = "Header creation failed." });

        return Ok(updatedList); // Returns updated list of headers
    }

    // Add Details to Order and Return Updated Full Order (Header + Details)
    [HttpPost("add-details/{orderId}")]
    public async Task<ActionResult<PurchaseOrderResponseDto>> AddDetails(int orderId, [FromBody] List<PurchaseOrderDetailDto> details)
    {
        var fullOrder = await _service.AddOrderDetailsAsync(orderId, details); // Call service to add details and return full order (header + details)

        if (fullOrder == null)
        {
            return NotFound(new ErrorResponseDto { Message = "Order ID not found or detail creation failed." });
        }

        return Ok(fullOrder); // Return updated full order (header + details)
    }
}
