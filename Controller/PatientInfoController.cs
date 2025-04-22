using AxonPDS.DTOs.PatientInfo;
using AxonPDS.Entities;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AxonPDS.Controller
{
    [Route("api/patient-info")]
    [ApiController]
    // [Authorize] 
    
    public class PatientInfoController(IPatientInfo _patientInfoRepo) : ControllerBase
    {
         private readonly IPatientInfo patientInfoRepo = _patientInfoRepo;

         // Get PatientInfo by Patient ID
        [HttpGet("{patientId:guid}")]
        public async Task<IActionResult> GetPatientInfo(Guid patientId)
        {
            var patientInfo = await patientInfoRepo.GetPatientInfo(patientId);
            if (patientInfo == null)
            {
                return NotFound(new { message = "Patient info not found" });
            }
            return Ok(new { patientInfo });
        }

        // Update or Create PatientInfo
        [HttpPut("{patientId:guid}")]
        public async Task<IActionResult> UpdatePatientInfo(Guid patientId, [FromBody] CreatePatientInfoDto patientInfo)
        {
            if (patientInfo == null)
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            var updatedPatientInfo = await patientInfoRepo.UpdatePatientInfo(patientId, patientInfo);

            if(updatedPatientInfo is null)
            {         
                return BadRequest(new { message = "Invalid request data" });
            }
            return Ok(new { message = "Patient info updated successfully", updatedPatientInfo });
        }
    }
}
