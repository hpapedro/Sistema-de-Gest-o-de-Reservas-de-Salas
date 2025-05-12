// Local: API/Repository/IAuditoriaRepository.cs
using API.Models;


namespace API.Repository // Certifique-se que este é o namespace correto
{
    public interface IAuditoriaRepository
    {

        // --- Métodos para auditoria de Reservas (usados pelo ReservaController) ---
        void RegistrarCriacaoReserva(Reserva reserva, int usuarioId);
        void RegistrarExclusaoReserva(Reserva reserva, int usuarioId); // Adicionado para completar o que o controller precisa

        List<Auditoria> ListarReservasCriadas();
        List<Auditoria> ListarReservasExcluidas();   

}
}