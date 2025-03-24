using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;

namespace Proyecto_camiones.Presentacion;

public class ApplicationDbContext : DbContext
{

    //Especie de entidad a la cual se agrega el nuevo objeto que será persistido en la bd
    public DbSet<Camion> Camiones { get; set; }
    public DbSet<Viaje> Viajes { get; set; }
    public DbSet<Cheque> Cheques { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Chofer> Choferes { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    // Constructor para pasar opciones (útil para pruebas o configuración)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }



    // Configurar la conexión a MySQL
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=truck_manager_project;user=root;password=",
                ServerVersion.AutoDetect("server=localhost;database=truck_manager_project;user=root;password="));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración adicional que mapea una entidad a su tabla en la base de datos
        modelBuilder.Entity<Camion>().ToTable("camion");
        modelBuilder.Entity<Viaje>().ToTable("viaje");
        modelBuilder.Entity<Cheque>().ToTable("cheque");
        modelBuilder.Entity<Pago>().ToTable("pago");
        modelBuilder.Entity<Chofer>().ToTable("chofer");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Usuario>().ToTable("usuario");

        modelBuilder.Entity<Camion>(entity =>
        {
            entity.ToTable("camion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcamion");
            entity.Property(e => e.peso_max).HasColumnName("peso_max");
            entity.Property(e => e.tara).HasColumnName("tara");
            entity.Property(e => e.Patente).HasColumnName("patente");
        });

        base.OnModelCreating(modelBuilder);

    }

}

