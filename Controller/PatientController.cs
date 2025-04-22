using AxonPDS.DTOs.Patient;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/patient")]
    [ApiController]
    [Authorize]
    public class PatientController(IPatient _patientRepo) : ControllerBase
    {
        private readonly IPatient patientRepo = _patientRepo;

         // Get Patient By Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var patient = await patientRepo.GetPatient(id);
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }
            return Ok(new { patient });
        }

        // Get All Patients with filtering, pagination, and sorting
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPatients(
            [FromQuery] int page = 1, 
            [FromQuery] int limit = 20, 
            [FromQuery] string gender = "all", 
            [FromQuery] string? dateRange = null, 
            [FromQuery] string sort = "newest", 
            [FromQuery] string search = "")
        {
            // Ensure empty strings are replaced with default values
            gender = string.IsNullOrWhiteSpace(gender) ? "all" : gender;
            dateRange = string.IsNullOrWhiteSpace(dateRange) ? null : dateRange;
            sort = string.IsNullOrWhiteSpace(sort) ? "newest" : sort;
            search = string.IsNullOrWhiteSpace(search) ? "" : search;

            // Ensure page and limit are valid numbers
            page = page <= 0 ? 1 : page;
            limit = limit <= 0 ? 20 : limit;

            var (patients, totalPatients) = await patientRepo.GetAllPatients(gender, dateRange, sort, page, limit, search);

            return Ok(new
            {
                patients,
                totalPatients,
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)totalPatients / limit)
            });
        }

        // Create a new patient
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto patient)
        {
            var isDuplicate = await patientRepo.IsDuplicateAsync(patient.Email, patient.Phonenumber);

            if (isDuplicate)
            {
                return BadRequest(new { success = false, error = "Email or Phonenumber Already Exist" });
            }

            var created = await patientRepo.CreatePatient(patient);
            if (created is null)
            {
                return BadRequest(new { error = "Failed to create patient" });
            }
            return CreatedAtAction(nameof(GetPatient), new { id = created.Id }, created);
        }

        // Update an existing patient
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientDto patient)
        {

            var updated = await patientRepo.UpdatePatient(id, patient);
            if (!updated)
            {
                return NotFound(new { success = false, error = "Patient not found" });
            }
            return Ok(new { success = true, message = "Patient updated successfully" });
        }

        // Delete a patient
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var hasRecords = await patientRepo.HasRecords(id);
            if (hasRecords)
            {
                return BadRequest(new { success = false, error = "Cannot delete patient with existing records" });
            }

            var deleted = await patientRepo.DeletePatient(id);
            if (!deleted)
            {
                return NotFound(new { success = false, error = "Patient not found" });
            }

            return Ok(new { success = true, message = "Patient deleted successfully" });
        }


    }
}
 