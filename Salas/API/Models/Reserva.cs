using System;

namespace API.Models;

public class Reserva
{
    public int Id {get; set;}
    public DateTime DataHoraInicio {get; set;}
    
    public DateTime DataHoraFim {get; set;}

    //CHAVES ESTRANGEIRAS

    public int SalaId {get; set;}
    public Sala? Sala {get; set;}
    public int UsuarioId{get; set;}
    public Usuario? Usuario {get; set;}
}

