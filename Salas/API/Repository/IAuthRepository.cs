using API.Models;

namespace API.Repository
{
    public interface IAuthRepository
    {
        Usuario? Autenticar(string email, string senha);
        string GerarToken(Usuario usuario);
    }
}
