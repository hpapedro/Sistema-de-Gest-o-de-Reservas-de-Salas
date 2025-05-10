using API.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint público (sem autenticação)
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API Funcionando");
        }

        // Endpoint protegido (requer autenticação)
        [HttpGet("database-check")]
        [Authorize] // Só acessa com token JWT válido
        public IActionResult DatabaseCheck()
        {
            try
            {
                var totalSalas = _context.Salas.Count();
                return Ok($"Banco de dados conectado! Total de salas: {totalSalas}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
            }
        }
    }
}