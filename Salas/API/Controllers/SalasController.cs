using API.data;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SalasController : ControllerBase{

    private readonly ISalaRepository _repository;
    public SalasController(ISalaRepository repository){
        _repository = repository;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    public ActionResult <List<Sala>> ListarSalas(){
        var todasAsSalas = _repository.ListarTodas();
        return Ok(todasAsSalas);
    }

    [HttpPost("registrar")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Sala> RegistrarSala([FromBody] Sala sala){
        if (_repository.VerificarSeNomeExiste(sala.Nome))
            return Conflict("Já existe uma sala com este nome");
        
        if (sala.Capacidade <= 0)
            return BadRequest("Capacidade deve ser maior que zero");

        _repository.Adicionar(sala);
        return CreatedAtAction(nameof(ListarSalas), new {id = sala.Id}, sala);
    }

    [HttpPut("editar/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditarSala(int id, [FromBody] Sala salaAtualizada){
        var salaExistente =  _repository.BuscarPorId(id);
        if (salaExistente == null)
            return NotFound("Sala não Encontrada");
        
        if (salaAtualizada.Capacidade <= 0)
            return BadRequest("Capacidade deve ser maior que zero");
        
        if (_repository.VerificarSeNomeExisteComIdDiferente(salaAtualizada.Nome, id))
            return Conflict("Já existe uma sala com este nome");

        salaExistente.Nome = salaAtualizada.Nome;
        salaExistente.Capacidade = salaAtualizada.Capacidade;

        _repository.Atualizar(salaExistente);

        return Ok(salaExistente);
    }

    [HttpDelete("remover/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Remover(int id){
        var sala = _repository.BuscarPorId(id);
        if (sala == null) 
            return NotFound("Sala não encontrada");

        _repository.Remover(id);
        return Ok("Sala Deletada com sucesso");
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult Buscar (int id){
        var sala = _repository.BuscarPorId(id);
        if (sala == null)
            return NotFound("Sala não encontrada");
        return Ok(sala);
    }
}