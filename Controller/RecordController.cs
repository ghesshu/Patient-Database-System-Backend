using AxonPDS.DTOs.Record;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/record")]
    [ApiController]
    // [Authorize]
    public class RecordController (IRecord _recordRepo) : ControllerBase
    {
        private readonly IRecord recordRepo = _recordRepo;

    [HttpPost]
    public async Task<IActionResult> CreateRecord([FromBody] CreateRecordDto dto)
    {
        var (success, message, record) = await recordRepo.CreateRecordAsync(dto);
        if (!success) return BadRequest(new { error = message, success });

        return CreatedAtAction(nameof(GetRecordByPatientId), new { patientId = record?.PatientId }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(Guid id, [FromBody] CreateRecordDto dto)
    {
        var (success, message, updatedRecord) = await recordRepo.UpdateRecordAsync(id, dto);
        if (!success) return NotFound(new { error = message, success });

        return Ok(updatedRecord);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(Guid id)
    {
        var (success, message) = await recordRepo.DeleteRecordAsync(id);
        if (!success) return NotFound(new { error = message, success });

        return NoContent();
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetRecordByPatientId(Guid patientId)
    {
        var (success, message, records) = await recordRepo.GetRecordsByPatientIdAsync(patientId);
        if (!success) return NotFound(new { error = message, success });

        return Ok(records);
    }
        
    }
}
