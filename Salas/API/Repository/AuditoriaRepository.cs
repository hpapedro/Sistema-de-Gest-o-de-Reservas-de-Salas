
using API.data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AppDbContext _context;

        public AuditoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Métodos para Reservas
        public void RegistrarCriacaoReserva(Reserva reserva, int usuarioId)
        {
            var auditoria = new Auditoria
            {
                Entidade = "Reserva",
                Acao = "Criar",
                Detalhes = $"Reserva criada para a sala {reserva.Sala?.Nome} (Início: {reserva.DataHoraInicio}, Fim: {reserva.DataHoraFim})",
                CriadoEm = DateTime.Now,
                UsuarioId = usuarioId,
                ReservaId = reserva.Id
            };

            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

        public void RegistrarAtualizacaoReserva(Reserva reservaOriginal, Reserva reservaAtualizada, int usuarioId)
        {
            var mudancas = new List<string>();
            
            if (reservaOriginal.DataHoraInicio != reservaAtualizada.DataHoraInicio)
                mudancas.Add($"Data/Hora Início: {reservaOriginal.DataHoraInicio} → {reservaAtualizada.DataHoraInicio}");
            
            if (reservaOriginal.DataHoraFim != reservaAtualizada.DataHoraFim)
                mudancas.Add($"Data/Hora Fim: {reservaOriginal.DataHoraFim} → {reservaAtualizada.DataHoraFim}");
            
            if (reservaOriginal.SalaId != reservaAtualizada.SalaId)
                mudancas.Add($"Sala: {reservaOriginal.SalaId} → {reservaAtualizada.SalaId}");

            var auditoria = new Auditoria
            {
                Entidade = "Reserva",
                Acao = "Atualizar",
                Detalhes = mudancas.Any() 
                    ? $"Reserva atualizada. Mudanças:\n{string.Join("\n", mudancas)}" 
                    : "Reserva atualizada (sem mudanças detectadas)",
                CriadoEm = DateTime.Now,
                UsuarioId = usuarioId,
                ReservaId = reservaAtualizada.Id
            };

            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

        public void RegistrarExclusaoReserva(Reserva reserva, int usuarioId)
        {
            var auditoria = new Auditoria
            {
                Entidade = "Reserva",
                Acao = "Excluir",
                Detalhes = $"Reserva excluída para a sala {reserva.Sala?.Nome} (Início: {reserva.DataHoraInicio}, Fim: {reserva.DataHoraFim})",
                CriadoEm = DateTime.Now,
                UsuarioId = usuarioId,
                ReservaId = reserva.Id
            };

            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

                public List<Auditoria> ListarReservasCriadas()
        {
            return _context.Auditorias
                .Where(a => a.Entidade == "Reserva" && a.Acao == "Criar")
                .Include(a => a.Usuario) // Para incluir dados do usuário, se quiser
                .Include(a => a.Reserva) // Para incluir dados da reserva
                    .ThenInclude(r => r.Sala) // E da sala dentro da reserva
                .OrderByDescending(a => a.CriadoEm)
                .ToList();
        }

        public List<Auditoria> ListarReservasExcluidas()
        {
            return _context.Auditorias
                .Where(a => a.Entidade == "Reserva" && a.Acao == "Excluir")
                .Include(a => a.Usuario)
                .Include(a => a.Reserva) 
                    .ThenInclude(r => r.Sala)
                .OrderByDescending(a => a.CriadoEm)
                .ToList();
        }
    
    }
}
