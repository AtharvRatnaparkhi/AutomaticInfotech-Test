using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApplication.DTO;
using TestApplication.Interfaces;

public class VendorService : IVendorService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public VendorService(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    // Get All Vendors
    public async Task<VendorListResponseDto> GetAllAsync()
    {
        using var dbConnection = new SqlConnection(_connectionString);
        await dbConnection.OpenAsync();

        var vendors = await dbConnection.QueryAsync<VendorDto>("sp_GetAllVendors", commandType: CommandType.StoredProcedure);
        return new VendorListResponseDto { Vendors = vendors.ToList() };
    }


    // Create Vendor
    public async Task<VendorListResponseDto> CreateAsync(CreateVendorDto vendor)
    {
        using (var dbConnection = new SqlConnection(_connectionString))
        {
            await dbConnection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("Code", vendor.Code);
            parameters.Add("Name", vendor.Name);
            parameters.Add("AddressLine1", vendor.AddressLine1);
            parameters.Add("AddressLine2", vendor.AddressLine2);
            parameters.Add("ContactEmail", vendor.ContactEmail);
            parameters.Add("ContactNo", vendor.ContactNo);
            parameters.Add("ValidTillDate", vendor.ValidTillDate);
            parameters.Add("IsActive", vendor.IsActive);

            await dbConnection.ExecuteScalarAsync<int>("sp_CreateVendor", parameters, commandType: CommandType.StoredProcedure);

            // Fetch the updated list of vendors
            var vendorList = await dbConnection.QueryAsync<VendorDto>("sp_GetAllVendors", commandType: CommandType.StoredProcedure);

            return new VendorListResponseDto
            {
                Vendors = vendorList.ToList()
            };
        }
    }

    // Update Vendor
    public async Task<VendorListResponseDto> UpdateAsync(UpdateVendorDto vendor)
    {
        using var dbConnection = new SqlConnection(_connectionString);
        await dbConnection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("Id", vendor.Id);
        parameters.Add("Code", vendor.Code);
        parameters.Add("Name", vendor.Name);
        parameters.Add("AddressLine1", vendor.AddressLine1);
        parameters.Add("AddressLine2", vendor.AddressLine2);
        parameters.Add("ContactEmail", vendor.ContactEmail);
        parameters.Add("ContactNo", vendor.ContactNo);
        parameters.Add("ValidTillDate", vendor.ValidTillDate);
        parameters.Add("IsActive", vendor.IsActive);

        var result = await dbConnection.ExecuteAsync("sp_UpdateVendor", parameters, commandType: CommandType.StoredProcedure);

        var updatedList = await dbConnection.QueryAsync<VendorDto>("sp_GetAllVendors", commandType: CommandType.StoredProcedure);
        return new VendorListResponseDto { Vendors = updatedList.ToList() };
    }

    // Delete Vendor
    public async Task<VendorListResponseDto> DeleteAsync(int id)
    {
        using var dbConnection = new SqlConnection(_connectionString);
        await dbConnection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        await dbConnection.ExecuteAsync("sp_DeleteVendor", parameters, commandType: CommandType.StoredProcedure);

        var updatedList = await dbConnection.QueryAsync<VendorDto>("sp_GetAllVendors", commandType: CommandType.StoredProcedure);
        return new VendorListResponseDto { Vendors = updatedList.ToList() };
    }

    // Get Vendor by Id
    public async Task<VendorDto> GetByIdAsync(int id)
    {
        using (var dbConnection = new SqlConnection(_connectionString))
        {
            await dbConnection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var vendor = await dbConnection.QuerySingleOrDefaultAsync<VendorDto>("sp_GetVendorById", parameters, commandType: CommandType.StoredProcedure);

            return vendor;
        }
    }

}
