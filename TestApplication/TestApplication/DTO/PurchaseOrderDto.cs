namespace TestApplication.DTO
{
    public class PurchaseOrderHeaderCreateDto
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int VendorId { get; set; }
        public string Notes { get; set; }
        public decimal OrderValue { get; set; }
        public bool IsActive { get; set; }
    }

    public class PurchaseOrderDetailDto
    {
        public int MaterialId { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemRate { get; set; }
        public string ItemNotes { get; set; }
        public DateTime ExpectedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class PurchaseOrderResponseDto
    {
        public PurchaseOrderHeaderDto Header { get; set; }
        public List<PurchaseOrderDetailDto> Details { get; set; }
    }
    public class PurchaseOrderCreateDto
    {
        public PurchaseOrderHeaderDto Header { get; set; }
        public List<PurchaseOrderDetailDto> Details { get; set; }
    }


    public class PurchaseOrderListResponseDto
    {
        public List<PurchaseOrderHeaderDto> Orders { get; set; }
    }
    public class PurchaseOrderHeaderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int VendorId { get; set; }
        public string Notes { get; set; }
        public decimal OrderValue { get; set; }
        public bool IsActive { get; set; }
    }
    public class ErrorResponseDto
    {
        public string Message { get; set; }
    }

}
