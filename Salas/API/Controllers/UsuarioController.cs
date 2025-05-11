using API.data;
using API.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuariosController(IUsuarioRepository _repository)
    {
        _usuarioRepository = _repository;
    }

    //POST PARA REGISTRAR USUARIOS

    [HttpPost("registrar")]
    public ActionResult<Usuario> RegistrarUsuario([FromBody] Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
        return BadRequest("Email e senha são obrigatórios");

        if ( _usuarioRepository.VerificarSeEmailExiste(usuario.Email))
        return Conflict("Email já cadastrado");

        usuario.Role ??= "User";

        _usuarioRepository.Registrar(usuario);

        return CreatedAtAction(nameof(RegistrarUsuario), new { id = usuario.Id}, usuario);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Usuario> BuscarPorId(int id)
    {
        var usuario = _usuarioRepository.BuscarPorId(id);
        if (usuario == null)
        return NotFound("Usuário não encontrado");

        return Ok(usuario);
    }
}