using System;
using API.Models;

namespace API.Repository;

public interface IUsuarioRepository
{
    void Registrar(Usuario usuario);
    Usuario? BuscarPorEmail(string email);
    bool VerificarSeEmailExiste(string email);
    Usuario? BuscarPorId(int id);
}
