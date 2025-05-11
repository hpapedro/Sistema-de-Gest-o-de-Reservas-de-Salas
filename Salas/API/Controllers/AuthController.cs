using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] Usuario usuarioLogin)
    {
        var usuario = _authRepository.Autenticar(usuarioLogin.Email, usuarioLogin.Senha);

        if (usuario == null)
            return Unauthorized("Email ou senha inválidos");

        var token = _authRepository.GerarToken(usuario);

        return Ok(new
        {
            message = "Login bem-sucedido",
            nome = usuario.Nome,
            role = usuario.Role,
            token
        });
    }
}
