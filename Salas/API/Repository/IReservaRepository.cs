using System;
using API.Models;

namespace API.Repository;

public interface IReservaRepository
{
    void Adicionar (Reserva reserva);
    List<Reserva> ListarPorUsuarioId(int usuarioId);
    List<Reserva> ListarTodas();
    bool VerificarConflito( int salaId, DateTime dataInicio, DateTime dataFim);
    Reserva? BuscarPorId(int id);
    void Remover(int id);
}
