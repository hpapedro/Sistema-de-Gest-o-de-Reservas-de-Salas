using System;
using API.data;
using API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace API.Data;

public class SalaRepository : ISalaRepository
{
    private readonly AppDbContext _context;

    public SalaRepository(AppDbContext context){
        _context = context;
    }

    public List<Sala> ListarTodas()
    {
        return _context.Salas.ToList();
    }

    public Sala? BuscarPorId(int id)
    {
        return _context.Salas.Find(id);
    }

    public void Adicionar (Sala sala)
    {
        _context.Salas.Add(sala);
        _context.SaveChanges();
    }

    public void Atualizar(Sala sala)
    {
        if (sala == null){
            throw new ArgumentNullException(nameof(sala), "A sala fornecida não pode ser nula");
        }
        _context.Salas.Update(sala);
        _context.SaveChanges();
    }

    public void Remover (int id)
    {
        var salaParaRemover = _context.Salas.Find(id);
        if (salaParaRemover != null){
        _context.Salas.Remove(salaParaRemover);
        _context.SaveChanges();
        }
    }

    public bool VerificarSeNomeExiste(string nome)
    {
        return _context.Salas.Any(s => s.Nome.ToUpper() == nome.ToUpper());
    }
    public bool VerificarSeNomeExisteComIdDiferente(string nome, int id)
    {
        return _context.Salas.Any(s => s.Nome.ToUpper() == nome.ToUpper() && s.Id != id);
    }

}
