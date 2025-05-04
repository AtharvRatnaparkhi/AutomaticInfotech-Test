namespace TestApplication.DTO
{
    public class GetAllVendorsDto
    {
        // Can add filtering fields like name, isActive, etc.
        public string SearchTerm { get; set; }
        public bool? IsActive { get; set; }
    }

}
