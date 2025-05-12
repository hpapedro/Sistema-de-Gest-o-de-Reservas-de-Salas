using System;

namespace API.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        public string Entidade { get; set; } // "Reserva", "Sala", "Usuario"
        public string Acao { get; set; } // "Criar", "Atualizar", "Excluir"
        public string Detalhes { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        
        // Relacionamento com o usuário que realizou a ação
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        // Chaves estrangeiras para as entidades relacionadas
        public int? ReservaId { get; set; }
        public Reserva Reserva { get; set; }
        
        public int? SalaId { get; set; }
        public Sala Sala { get; set; }
        
        public int? UsuarioAlvoId { get; set; }
        public Usuario UsuarioAlvo { get; set; }
    }
}