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

    private string GenerateToken(Usuario usuario){
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
