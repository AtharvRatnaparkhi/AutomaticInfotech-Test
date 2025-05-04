using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Interfaces;
using System.Threading.Tasks;

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVendors()
        {
            var vendors = await _vendorService.GetAllAsync();
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return Ok(vendor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDto vendor)
        {
            if (vendor == null)
                return BadRequest("Vendor data is required.");

            var vendorList = await _vendorService.CreateAsync(vendor);
            return Ok(vendorList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] UpdateVendorDto vendor)
        {
            if (vendor == null || vendor.Id != id)
                return BadRequest();

            var updatedList = await _vendorService.UpdateAsync(vendor);
            return Ok(updatedList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var updatedList = await _vendorService.DeleteAsync(id);
            return Ok(updatedList);
        }

    }
}
