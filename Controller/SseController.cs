using AxonPDS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AxonPDS.Controller
{
    [Route("api/sse")]
    [ApiController]
    public class SseController : ControllerBase
    {
        private readonly SseRepo _repository;

        public SseController(SseRepo repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public async Task GetStream(CancellationToken cancellationToken)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");

            await foreach (var data in _repository.StreamDataAsync(cancellationToken))
            {
                await Response.WriteAsync($"data: {System.Text.Json.JsonSerializer.Serialize(data)}\n\n");
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
    }
}