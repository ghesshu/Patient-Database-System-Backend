using AxonPDS.DTOs.MedicineRecord;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AxonPDS.Controller
{
    [Route("api/medicine-record")]
    [ApiController]
    // [Authorize]
    public class MedicineRecordController(IMedicineRecord medicineRecordRepo) : ControllerBase
    {
        private readonly IMedicineRecord _medicineRecordRepo = medicineRecordRepo;

        // Create a New Medicine Record
        [HttpPost]
        public async Task<IActionResult> CreateMedicineRecord([FromBody] CreateMedicineRecordDto dto)
        {
            (bool success, string message) = await _medicineRecordRepo.CreateMRecord(dto);

            if (!success)
            {
                return BadRequest(new { message, success });
            }

            return Ok(new { message, success });
        }

        // Delete Medicine Record
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMedicineRecord(Guid id)
        {
            var (success, message) = await _medicineRecordRepo.DeleteMRecord(id);

            if (!success)
            {
                return NotFound(new { message, success });
            }

            return Ok(new { message, success });
        }
    }
}
