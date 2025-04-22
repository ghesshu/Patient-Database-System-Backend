using AxonPDS.DTOs.Treatment;
using AxonPDS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AxonPDS.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class TreatmentsController(ITreatment treatmentRepo) : ControllerBase
    {
        private readonly ITreatment _treatmentRepo = treatmentRepo;
        // Create Treatment
        [HttpPost]
        public async Task<IActionResult> CreateTreatment([FromBody] CreateTreatmentDto dto)
        {
            var treatment = await _treatmentRepo.CreateTreatmentAsync(dto);
            return CreatedAtAction(nameof(GetAllTreatments), new 
            { 
                message = "Treatment Created Succesfully", 
                success = true ,
                treatment
            });
        }

         // Update Treatment
        // [HttpPut("{id}")]
        // public async Task<IResult> UpdateTreatment(Guid id, [FromBody] UpdateTreatmentDto dto)
        // {
        //     var (success, message) = await _treatmentRepo.UpdateTreatmentAsync(id, dto);
            
        //     if (!success)
        //     {
        //         return Results.Ok(new{success = true});

        //         return NotFound(new 
        //         {
        //             error = message,
        //             success = false
        //         });
        //     }

        //     return Ok(new 
        //     {
        //         message,
        //         success = true
        //     });
        // }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTreatment(Guid id, [FromBody] UpdateTreatmentDto dto)
        {
            var (success, message) = await _treatmentRepo.UpdateTreatmentAsync(id, dto);
            
            if (!success)
            {
                // return Results.Ok(new{success = true});

                return NotFound(new 
                {
                    error = message,
                    success = false
                });
            }

            return Ok(new 
            {
                message,
                success = true
            });
        }

        // Delete Treatment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreatment(Guid id)
        {
            
           var (success, message) = await _treatmentRepo.DeleteTreatmentAsync(id);
    
            if (!success)
            {
                return NotFound(new 
                {
                    error = message,
                    success = false
                });
            }

            return Ok(new 
            {
                message,
                success = true
            });
        }


        // Get All Treatments
        [HttpGet]
        public async Task<IActionResult> GetAllTreatments([
            FromQuery] string? search = "", 
            [FromQuery] string status = "all", 
            [FromQuery] int page = 1, 
            [FromQuery] int limit = 20)
        {
            var (treatments, totalTreatments) = await _treatmentRepo.GetAllTreatmentsAsync(search, status, page, limit);
            return Ok(new 
            { 
                treatments, 
                totalTreatments,
                currentPage = page,
                totalPages = (int)Math.Ceiling((double) totalTreatments / limit),
                success = true

            });
        }
    }
}
