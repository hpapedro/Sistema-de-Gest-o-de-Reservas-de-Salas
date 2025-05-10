using System.Xml;
using API.data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    //POST PARA REGISTRAR USUARIOS

    [HttpPost("registrar")]
    public async Task<ActionResult<Usuario>> RegistrarUsuario([FromBody] Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
        return BadRequest("Email e senha são obrigatórios");

        if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
        return Conflict("Email já cadastrado");

        usuario.Role ??= "User";

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(RegistrarUsuario), new { id = usuario.Id}, usuario);
    }
}