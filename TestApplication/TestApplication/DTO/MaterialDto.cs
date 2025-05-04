namespace TestApplication.DTO
{
    public class MaterialDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? ShortText { get; set; }
        public string? LongText { get; set; }
        public string? Unit { get; set; }
        public int ReorderLevel { get; set; }
        public int MinOrderQty { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateMaterialDto
    {
        public string? Code { get; set; }
        public string? ShortText { get; set; }
        public string? LongText { get; set; }
        public string? Unit { get; set; }
        public int ReorderLevel { get; set; }
        public int MinOrderQty { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateMaterialDto : CreateMaterialDto
    {
        public int ID { get; set; }
    }

    public class MaterialListResponseDto
    {
        public List<MaterialDto> Materials { get; set; }
    }
}
