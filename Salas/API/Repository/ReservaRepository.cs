using System;
using API.data;
using API.Models;

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
                            .Where (r => r.UsuarioId == usuarioId)
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
}
