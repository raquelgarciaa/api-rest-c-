using api_with_redis.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace api_with_redis.Controllers
{
    [ApiController]
    [Route("api/cache/")]
    public class HomeController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        public HomeController(IDistributedCache cache)
        {
        _cache = cache;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Request request)
        {
            var valor = await _cache.GetStringAsync(request.id);
            bool existe = valor is not null;

            if (existe)
            {
                return BadRequest($"A chave '{request.id}' já existe.");
            }

            await _cache.SetStringAsync(request.id, request.url);
            return Created(string.Empty, new { request.id, request.url });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var valor = await _cache.GetStringAsync(id);
            bool existe = valor is not null;

            if (existe)
            {
                return Ok(valor);
            }

            return NotFound();
        }
    }
}
