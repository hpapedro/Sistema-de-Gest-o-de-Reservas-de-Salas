using API.data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class ReservaRepository : IReservaRepository
{
    private readonly AppDbContext _context;

    public ReservaRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Adicionar(Reserva reserva)
    {
        if (reserva == null)
            throw new ArgumentNullException(nameof(reserva), "A Sala nao pode ser vazia");

        _context.Reservas.Add(reserva);
        _context.SaveChanges();
    }

    public List<Reserva> ListarPorUsuarioId(int usuarioId)
    {
        return _context.Reservas
                            .Where(r => r.UsuarioId == usuarioId)
                            .ToList();
    }

    public List<Reserva> ListarTodas()
    {
        return _context.Reservas.ToList();
    }

    public bool VerificarConflito(int salaId, DateTime dataInicio, DateTime dataFim)
    {
        if (dataFim <= dataInicio)
        {
            return false;
        }

        bool existeConflito = _context.Reservas.Any(reservaExistente =>
            reservaExistente.SalaId == salaId &&
            reservaExistente.DataHoraInicio < dataFim &&
            reservaExistente.DataHoraFim > dataInicio
        );


        return existeConflito;
    }

public void Remover(int id)
{
    // Primeiro, remover auditorias relacionadas à reserva
    var auditoriasRelacionadas = _context.Auditorias.Where(a => a.ReservaId == id);
    _context.Auditorias.RemoveRange(auditoriasRelacionadas);

    // Depois, remover a reserva
    var reservaParaRemover = _context.Reservas.Find(id);
    if (reservaParaRemover != null)
    {
        _context.Reservas.Remove(reservaParaRemover);
    }

    _context.SaveChanges();
}


public Reserva? BuscarPorId(int id)
{
    return _context.Reservas
        .Include(r => r.Sala)
        .FirstOrDefault(r => r.Id == id);
}
}

