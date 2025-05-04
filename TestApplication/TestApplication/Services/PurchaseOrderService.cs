using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApplication.DTO;
using TestApplication.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public PurchaseOrderService(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    // Get All Orders
    public async Task<List<PurchaseOrderHeaderDto>> GetAllOrdersAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return (await connection.QueryAsync<PurchaseOrderHeaderDto>("sp_GetAllPurchaseOrders", commandType: CommandType.StoredProcedure)).AsList();
    }

    // Get Order by ID
    public async Task<PurchaseOrderResponseDto> GetOrderByIdAsync(int orderId)
    {
        using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", orderId);

        var header = await connection.QuerySingleOrDefaultAsync<PurchaseOrderHeaderDto>("sp_GetPurchaseOrderHeaderById", parameters, commandType: CommandType.StoredProcedure);
        var details = await connection.QueryAsync<PurchaseOrderDetailDto>("sp_GetPurchaseOrderDetailsByOrderId", parameters, commandType: CommandType.StoredProcedure);

        if (header == null) return null;

        return new PurchaseOrderResponseDto
        {
            Header = header,
            Details = details.AsList()
        };
    }

    // Create Order Header
    public async Task<List<PurchaseOrderHeaderDto>> CreateOrderHeaderAsync(PurchaseOrderHeaderCreateDto header)
    {
        using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@OrderNumber", header.OrderNumber);
        parameters.Add("@OrderDate", header.OrderDate);
        parameters.Add("@VendorId", header.VendorId);
        parameters.Add("@Notes", header.Notes);
        parameters.Add("@OrderValue", header.OrderValue);
        parameters.Add("@IsActive", header.IsActive);

        // Add OUTPUT parameter
        parameters.Add("@NewOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("sp_CreatePurchaseOrderHeader", parameters, commandType: CommandType.StoredProcedure);

        // Optional: retrieve the newly created Order ID
        int newOrderId = parameters.Get<int>("@NewOrderId");

        // If you want to return this ID instead of all orders:
        // return await GetOrderByIdAsync(newOrderId);

        return await GetAllOrdersAsync();
    }


    // Add Order Details
    public async Task<PurchaseOrderResponseDto> CreateOrderDetailAsync(int orderId, List<PurchaseOrderDetailDto> details)
    {
        using var connection = new SqlConnection(_connectionString);

        foreach (var detail in details)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);
            parameters.Add("@MaterialId", detail.MaterialId);
            parameters.Add("@ItemQuantity", detail.ItemQuantity);
            parameters.Add("@ItemRate", detail.ItemRate);
            parameters.Add("@ItemNotes", detail.ItemNotes);
            parameters.Add("@ExpectedDate", detail.ExpectedDate);
            parameters.Add("@IsActive", detail.IsActive);

            // Execute stored procedure to insert each detail
            await connection.ExecuteAsync("sp_CreatePurchaseOrderDetail", parameters, commandType: CommandType.StoredProcedure);
        }

        // After adding details, return the full order (header + details)
        return await GetOrderByIdAsync(orderId);
    }

    // Add Details to an Order and Return Updated List of Orders
    public async Task<PurchaseOrderResponseDto> AddOrderDetailsAsync(int orderId, List<PurchaseOrderDetailDto> details)
    {
        return await CreateOrderDetailAsync(orderId, details);
    }

    // Update Order
    public async Task<bool> UpdateOrderAsync(int id, PurchaseOrderHeaderDto header, List<PurchaseOrderDetailDto> details)
    {
        using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", id);
        parameters.Add("@OrderNumber", header.OrderNumber);
        parameters.Add("@OrderDate", header.OrderDate);
        parameters.Add("@VendorId", header.VendorId);
        parameters.Add("@Notes", header.Notes);
        parameters.Add("@OrderValue", header.OrderValue);
        parameters.Add("@IsActive", header.IsActive);

        var result = await connection.ExecuteAsync("sp_UpdatePurchaseOrderHeader", parameters, commandType: CommandType.StoredProcedure);

        if (result == 0) return false;

        // Update Details for the Order
        foreach (var detail in details)
        {
            var detailParameters = new DynamicParameters();
            detailParameters.Add("@OrderId", id);
            detailParameters.Add("@MaterialId", detail.MaterialId);
            detailParameters.Add("@ItemQuantity", detail.ItemQuantity);
            detailParameters.Add("@ItemRate", detail.ItemRate);
            detailParameters.Add("@ItemNotes", detail.ItemNotes);
            detailParameters.Add("@ExpectedDate", detail.ExpectedDate);
            detailParameters.Add("@IsActive", detail.IsActive);

            await connection.ExecuteAsync("sp_UpdatePurchaseOrderDetail", detailParameters, commandType: CommandType.StoredProcedure);
        }

        return true;
    }

    // Delete Order
    public async Task<bool> DeleteOrderAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@OrderId", id);

        var result = await connection.ExecuteAsync("sp_DeletePurchaseOrder", parameters, commandType: CommandType.StoredProcedure);

        return result > 0;
    }

    public async Task<bool> UpdateOrderAsync(int id, PurchaseOrderHeaderCreateDto header, List<PurchaseOrderDetailDto> details)
    {
        using var connection = new SqlConnection(_connectionString);

        // Begin a transaction
        using var transaction = connection.BeginTransaction();

        try
        {
            // Update the Purchase Order header
            var headerParams = new DynamicParameters();
            headerParams.Add("@Id", id);
            headerParams.Add("@OrderNumber", header.OrderNumber);
            headerParams.Add("@OrderDate", header.OrderDate);
            headerParams.Add("@VendorId", header.VendorId);
            headerParams.Add("@Notes", header.Notes);
            headerParams.Add("@OrderValue", header.OrderValue);
            headerParams.Add("@IsActive", header.IsActive);

            await connection.ExecuteAsync("sp_UpdatePurchaseOrderHeader", headerParams, transaction, commandType: CommandType.StoredProcedure);

            // Remove existing details for the order
            await connection.ExecuteAsync("sp_DeletePurchaseOrderDetails", new { OrderId = id }, transaction, commandType: CommandType.StoredProcedure);

            // Add new details (if any)
            foreach (var detail in details)
            {
                var detailParams = new DynamicParameters();
                detailParams.Add("@OrderId", id);
                detailParams.Add("@MaterialId", detail.MaterialId);
                detailParams.Add("@ItemQuantity", detail.ItemQuantity);
                detailParams.Add("@ItemRate", detail.ItemRate);
                detailParams.Add("@ItemNotes", detail.ItemNotes);
                detailParams.Add("@ExpectedDate", detail.ExpectedDate);
                detailParams.Add("@IsActive", detail.IsActive);

                await connection.ExecuteAsync("sp_CreatePurchaseOrderDetail", detailParams, transaction, commandType: CommandType.StoredProcedure);
            }

            // Commit the transaction
            transaction.Commit();
            return true;
        }
        catch (Exception)
        {
            // Rollback in case of error
            transaction.Rollback();
            return false;
        }
    }

}
