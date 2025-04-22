using AxonPDS.DTOs.TreatmentRecord;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/record-treatment")]
    [ApiController]

    public class TreatmentRecordController(ITreatmentRecord TRecordRepo) : ControllerBase
    {
        private readonly ITreatmentRecord _TRecordRepo = TRecordRepo;


        // Create Treatment Record
        [HttpPost]
        public async Task<IActionResult> UpdateRecords([FromBody] CreateTRecordDTO body)
        {
            var (success, Message) = await _TRecordRepo.UpdateTRecords(body);
            return success != null 
                ? Ok(new { message = Message, success = true })
                : BadRequest(new { message = "Something went wrong", success = false });
        }

       
    
    }
}

