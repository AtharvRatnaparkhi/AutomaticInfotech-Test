using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApplication.DTO;
using TestApplication.Interfaces;

public class MaterialService : IMaterialService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public MaterialService(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    public async Task<MaterialListResponseDto> GetAllAsync()
    {
        using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();
        var materials = await db.QueryAsync<MaterialDto>("sp_GetAllMaterials", commandType: CommandType.StoredProcedure);
        return new MaterialListResponseDto { Materials = materials.ToList() };
    }

    public async Task<MaterialDto?> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();
        var parameters = new DynamicParameters();
        parameters.Add("ID", id);
        return await db.QuerySingleOrDefaultAsync<MaterialDto>("sp_GetMaterialById", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<MaterialListResponseDto> CreateAsync(CreateMaterialDto material)
    {
        using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();
        var parameters = new DynamicParameters();
        parameters.Add("Code", material.Code);
        parameters.Add("ShortText", material.ShortText);
        parameters.Add("LongText", material.LongText);
        parameters.Add("Unit", material.Unit);
        parameters.Add("ReorderLevel", material.ReorderLevel);
        parameters.Add("MinOrderQty", material.MinOrderQty);
        parameters.Add("IsActive", material.IsActive);

        await db.ExecuteAsync("sp_CreateMaterial", parameters, commandType: CommandType.StoredProcedure);

        return await GetAllAsync();
    }

    public async Task<MaterialListResponseDto> UpdateAsync(UpdateMaterialDto material)
    {
        using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();
        var parameters = new DynamicParameters();
        parameters.Add("ID", material.ID);
        parameters.Add("Code", material.Code);
        parameters.Add("ShortText", material.ShortText);
        parameters.Add("LongText", material.LongText);
        parameters.Add("Unit", material.Unit);
        parameters.Add("ReorderLevel", material.ReorderLevel);
        parameters.Add("MinOrderQty", material.MinOrderQty);
        parameters.Add("IsActive", material.IsActive);

        await db.ExecuteAsync("sp_UpdateMaterial", parameters, commandType: CommandType.StoredProcedure);

        return await GetAllAsync();
    }

    public async Task<MaterialListResponseDto> DeleteAsync(int id)
    {
        using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();
        var parameters = new DynamicParameters();
        parameters.Add("ID", id);
        await db.ExecuteAsync("sp_DeleteMaterial", parameters, commandType: CommandType.StoredProcedure);

        return await GetAllAsync();
    }
}
