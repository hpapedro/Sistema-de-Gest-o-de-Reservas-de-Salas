using System;

namespace API.Models;

public class Auditoria
{
    public int Id {get; set;}
    public string Acao {get; set;} = string.Empty;
    public DateTime CridadoEm {get; set;} = DateTime.Now;

    //Relacionamento com Reserva
    public int ReservaId {get; set;}
    public Reserva? Reserva {get; set;}

    //Relacionamento com o Usuario
    public int UsuarioId{get; set;}
    public Usuario? Usuario{get; set;}
}
