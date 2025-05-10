using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Usuario> Usuarios {get; set;}
    public DbSet<Sala> Salas {get; set;}
    public DbSet<Reserva> Reservas {get; set;}
    public DbSet<Auditoria> Auditorias {get; set;}

}
