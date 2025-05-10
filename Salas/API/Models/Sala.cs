using System;

namespace API.Models;

public class Sala
{
    public int Id {get; set;}
    public string Nome {get; set;} = string.Empty;
    public int Capacidade {get; set;} 
    public ICollection<Reserva>? Reservas {get; set;}
}
