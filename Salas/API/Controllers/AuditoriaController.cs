using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaController(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

           [HttpGet("reservas/criadas")] // Rota: GET api/auditoria/reservas/criadas
        public ActionResult<List<Auditoria>> ListarAuditoriaReservasCriadas()
        {
            var auditorias = _auditoriaRepository.ListarReservasCriadas();
            if (auditorias == null || !auditorias.Any())
            {
                return NotFound("Nenhum registro de criação de reserva encontrado.");
            }
            return Ok(auditorias);
        }

        // Endpoint para listar auditorias de RESERVAS EXCLUÍDAS
        [HttpGet("reservas/excluidas")] // Rota: GET api/auditoria/reservas/excluidas
        public ActionResult<List<Auditoria>> ListarAuditoriaReservasExcluidas()
        {
            var auditorias = _auditoriaRepository.ListarReservasExcluidas();
            if (auditorias == null || !auditorias.Any())
            {
                return NotFound("Nenhum registro de exclusão de reserva encontrado.");
            }
            return Ok(auditorias);
        }
}
}