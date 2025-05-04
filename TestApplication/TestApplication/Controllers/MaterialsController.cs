using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Interfaces;

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _materialService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var material = await _materialService.GetByIdAsync(id);
            if (material == null) return NotFound();
            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMaterialDto dto)
        {
            if (dto == null) return BadRequest();
            var response = await _materialService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMaterialDto dto)
        {
            if (dto == null || dto.ID != id) return BadRequest();
            var response = await _materialService.UpdateAsync(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _materialService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
