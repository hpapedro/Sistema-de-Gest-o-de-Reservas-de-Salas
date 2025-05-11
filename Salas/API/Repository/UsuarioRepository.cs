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
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    
}
