using API.data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SalasController : ControllerBase{
    private readonly AppDbContext _context;

    public SalasController(AppDbContext context){
        _context = context;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Sala>>> ListarSalas(){
        return await _context.Salas.ToListAsync();
    }

    [HttpPost("registrar")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Sala>> RegistrarSala([FromBody] Sala sala){
        if (await _context.Salas.AnyAsync(s => s.Nome == sala.Nome))
            return Conflict("Já existe uma sala com este nome");
        
        if (sala.Capacidade <= 0)
            return BadRequest("Capacidade deve ser maior que zero");

        _context.Salas.Add(sala);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ListarSalas), new {id = sala.Id}, sala);

    }

    [HttpPut("editar/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditarSala(int id, [FromBody] Sala salaAtualizada){
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null)
            return NotFound("Sala não Encontrada");
        
        if (salaAtualizada.Capacidade <= 0)
            return BadRequest("Capacidade deve ser maior que zero");
        
        if (await _context.Salas.AnyAsync(s => s.Nome == salaAtualizada.Nome && s.Id != id))
            return Conflict("Já existe uma sala com este nome");

        sala.Nome = salaAtualizada.Nome;
        sala.Capacidade = salaAtualizada.Capacidade;

        await _context.SaveChangesAsync();

        return Ok(sala);
    }

    [HttpDelete("remover/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Remover(int id){
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) 
            return NotFound("Sala não encontrada");

        _context.Salas.Remove(sala);
        await _context.SaveChangesAsync();
            return Ok("Sala Deletada com sucesso");
    }
}