using System;
using API.data;
using API.Models;

namespace API.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context){
        _context = context;
    }

    public void Registrar (Usuario usuario)
    {
        if (usuario == null)
        throw new ArgumentNullException(nameof(usuario), "O usuario nao pode ser nulo");
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public Usuario? BuscarPorEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return null;

            return _context.Usuarios.FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
    }

    
    public bool VerificarSeEmailExiste(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return _context.Usuarios.Any(u => u.Email.ToUpper() == email.ToUpper());
    }
    public Usuario? BuscarPorId(int id)
    {
        return _context.Usuarios.Find(id);
    }

    
}
