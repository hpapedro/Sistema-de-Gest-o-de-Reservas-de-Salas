using System.Security.Claims;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ISalaRepository _salaRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;

        public ReservaController(
            IReservaRepository reservaRepository, 
            ISalaRepository salaRepository,
            IAuditoriaRepository auditoriaRepository)
        {
            _reservaRepository = reservaRepository;
            _salaRepository = salaRepository;
            _auditoriaRepository = auditoriaRepository;
        }

        private int GetUsuarioIdLogado()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int usuarioId))
                return usuarioId;

            throw new InvalidOperationException("Nao foi possivel obter o ID do usuario");
        }

        [HttpPost]
        public IActionResult CriarReserva([FromBody] Reserva reservaParaCriar)
        {
            int usuarioId;
            try
            {
                usuarioId = GetUsuarioIdLogado();
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("Usuario nao autorizado");
            }

            if (reservaParaCriar == null)
            {
                return BadRequest("Dados da reserva não fornecidos");
            }   

            reservaParaCriar.Id = 0;
            reservaParaCriar.UsuarioId = usuarioId;

            if (reservaParaCriar.SalaId <= 0)
            {  
                return BadRequest("ID da sala invalido");
            }

            if (reservaParaCriar.DataHoraFim <= reservaParaCriar.DataHoraInicio)
            {
                return BadRequest("A data/hora fim da reserva deve ser posterior a data/hora de inicio");
            }

            if (reservaParaCriar.DataHoraInicio < DateTime.Now){
                return BadRequest("A data/hora de inicio da reserva deve ser posterior a data atual");
            }

            var salaExistente = _salaRepository.BuscarPorId(reservaParaCriar.SalaId);
            if (salaExistente == null)
            {
                return NotFound($"Sala com o ID {reservaParaCriar.SalaId} nao encontrado");
            }

            if (_reservaRepository.VerificarConflito(reservaParaCriar.SalaId, reservaParaCriar.DataHoraInicio, reservaParaCriar.DataHoraFim))
            {
                return Conflict("Horario Indisponivel. Ja existe uma reserva nessa sala para esse mesmo periodo");
            }

            _reservaRepository.Adicionar(reservaParaCriar);

            try
            {
                _auditoriaRepository.RegistrarCriacaoReserva(reservaParaCriar, usuarioId);
            }
            catch (Exception)
            {
                Console.WriteLine("Erro ao registrar auditoria de criação");
            }
            return CreatedAtAction(null, new { id = reservaParaCriar.Id }, reservaParaCriar);
        }

        [HttpGet("minhas")]
        public ActionResult<List<Reserva>> ListarMinhasReservas()
        {
            int usuarioId;
            try
            {
                usuarioId = GetUsuarioIdLogado();
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("Usuario nao autorizado");
            }

            var minhasReservas = _reservaRepository.ListarPorUsuarioId(usuarioId);

            if (minhasReservas == null || !minhasReservas.Any())
            {
                return Ok("Voce ainda nao possui nenhuma reserva");
            }

            return Ok(minhasReservas);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public ActionResult<List<Reserva>> ListarTodas() 
        {
            var todasAsReservas = _reservaRepository.ListarTodas();
            return Ok(todasAsReservas);
        }

[HttpDelete("Remover/{id}")]
[Authorize(Roles = "Admin")]
public IActionResult Remover(int id)
{
    try
    {
        int usuarioIdLogado = GetUsuarioIdLogado();

        var reservaParaDeletar = _reservaRepository.BuscarPorId(id);
        if (reservaParaDeletar == null)
            return NotFound("Reserva nao encontrada");

        try
        {
            _auditoriaRepository.RegistrarExclusaoReserva(reservaParaDeletar, usuarioIdLogado);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro na auditoria: {e.Message}");
        }

        _reservaRepository.Remover(id);

        return Ok("Reserva Deletada com Sucesso");
    }
catch (Exception ex)
{
    string mensagemErro = ex.Message;
    if (ex.InnerException != null)
        mensagemErro += " | Inner: " + ex.InnerException.Message;

    return StatusCode(500, $"Erro inesperado: {mensagemErro}");
}
}

    }
}
