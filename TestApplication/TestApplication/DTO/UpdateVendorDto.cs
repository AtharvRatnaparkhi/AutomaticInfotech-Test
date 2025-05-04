namespace TestApplication.DTO
{
    public class UpdateVendorDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNo { get; set; }
        public DateTime ValidTillDate { get; set; }
        public bool IsActive { get; set; }
    }

}
