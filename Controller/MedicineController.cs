using AxonPDS.DTOs.Medicine;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/medicine")]
    [ApiController]
    // [Authorize]
    public class MedicineController(IMedicine medicineRepo) : ControllerBase
    {
        private readonly IMedicine _medicineRepo = medicineRepo;

        // Get All Medicines with Pagination, Search, and Stock Status Filtering
        [HttpGet]
        public async Task<IActionResult> GetAllMedicines(
            [FromQuery] string? search = "",
            [FromQuery] string status = "all",
            [FromQuery] int page = 1,
            [FromQuery] int limit = 20)
        {
            var (medicines, total) = await _medicineRepo.GetAllMedicines(search, status, page, limit);
            return Ok(new
            {
                medicines,
                totalFilteredMedicines = total,
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)total / limit)
            });
        }

        // Get a Single Medicine by ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMedicineById(Guid id)
        {
            var medicine = await _medicineRepo.GetMedicineById(id);
            if (medicine == null)
            {
                return NotFound(new { error = "Medicine not found" });
            }
            return Ok(medicine);
        }

        // Create a New Medicine
        [HttpPost]
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDto medicineDto)
        {
            var medicine = await _medicineRepo.CreateMedicine(medicineDto);
            if (medicine == null)
            {
                return StatusCode(500, new { error = "Failed to create medicine" });
            }
            return CreatedAtAction(nameof(GetMedicineById), new { id = medicine.Id }, medicine);
        }

        // Update Medicine Details
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMedicine(Guid id, [FromBody] UpdateMedicineDto medicineDto)
        {
            var medicine = await _medicineRepo.UpdateMedicine(id, medicineDto);
            if (!medicine)
            {
                return NotFound(new { error = "Medicine not found" });
            }
            return Ok(medicine);
        }

        // Delete Medicine
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMedicine(Guid id)
        {
            var (success, message) = await _medicineRepo.CanDeleteMedicine(id);
            if (!success)
            {
                return BadRequest(new { error = "Medicine cannot be deleted as it is used in a record" });
            }

            var deleted = await _medicineRepo.DeleteMedicine(id);
            if (!deleted)
            {
                return NotFound(new { error = "Medicine not found" });
            }

            return Ok(new { message = "Medicine deleted successfully" });
        }
    }
}
