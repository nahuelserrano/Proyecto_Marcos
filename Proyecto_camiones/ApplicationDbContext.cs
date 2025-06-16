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
    public DbSet<Sueldo> Sueldos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<CuentaCorriente> Cuentas { get; set; }
    public DbSet<ViajeFlete> ViajesFlete { get; set; }
    public DbSet<Flete> Fletes { get; set; }
    public DbSet<Chofer> Choferes { get; set; }

    // Constructor para pasar opciones (útil para pruebas o configuración)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }



    // Configurar la conexión a MySQL
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=truck_manager_project_db;user=root;password=",
                ServerVersion.AutoDetect("server=localhost;database=truck_manager_project_db;user=root;password="));

            Database.SetCommandTimeout(30);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración adicional que mapea una entidad a su tabla en la base de datos
        modelBuilder.Entity<Camion>().ToTable("camion");
        modelBuilder.Entity<Viaje>().ToTable("viaje");
        modelBuilder.Entity<Cheque>().ToTable("cheque");
        modelBuilder.Entity<Sueldo>().ToTable("pago");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Usuario>().ToTable("usuario");
        modelBuilder.Entity<CuentaCorriente>().ToTable("cuenta_corriente");
        modelBuilder.Entity<Chofer>().ToTable("chofer");

        modelBuilder.Entity<Camion>(entity =>
        {
            entity.ToTable("camion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcamion");
            entity.Property(e => e.Patente).HasColumnName("patente");
            entity.Property(e => e.nombre_chofer).HasColumnName("nombre_chofer");
        });

        modelBuilder.Entity<Viaje>(entity =>
        {
            entity.ToTable("viaje");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idviaje");
            entity.Property(e => e.FechaInicio).HasColumnName("partida");
            entity.Property(e => e.LugarPartida).HasColumnName("origen");
            entity.Property(e => e.Destino).HasColumnName("destino");
            entity.Property(e => e.Remito).HasColumnName("remito");
            entity.Property(e => e.Kg).HasColumnName("kg");
            entity.Property(e => e.Carga).HasColumnName("carga");
            entity.Property(e => e.Cliente).HasColumnName("idcliente");
            entity.Property(e => e.Camion).HasColumnName("idcamion");
            entity.Property(e => e.Km).HasColumnName("km");
            entity.Property(e => e.Tarifa).HasColumnName("tarifa");
            entity.Property(e => e.NombreChofer).HasColumnName("nombre_chofer");
            entity.Property(e => e.PorcentajeChofer).HasColumnName("comision_chofer");
            entity.HasOne(v => v.ClienteNavigation)
                .WithMany(c => c.Viajes)
                .HasForeignKey(v => v.Cliente)
                .OnDelete(DeleteBehavior.Restrict); // Impide que se elimine un cliente con viajes
            entity.HasOne(v => v.CamionNavigation)
                .WithMany(c => c.Viajes)
                .HasForeignKey(v => v.Camion)
                .OnDelete(DeleteBehavior.Restrict); // Impide que se elimine un camión con viajes
        });

        modelBuilder.Entity<CuentaCorriente>(entity =>
        {
            entity.ToTable("cuenta_corriente");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcuenta_corriente").ValueGeneratedOnAdd();
            entity.Property(e => e.Nro_factura).HasColumnName("nro_factura");
            entity.Property(e => e.Fecha_factura).HasColumnName("fecha_factura");
            entity.Property(e => e.Adeuda).HasColumnName("adeuda");
            entity.Property(e => e.Pagado).HasColumnName("importe_pagado");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdFletero).HasColumnName("idfletero");
            entity.Property(e => e.Saldo_Total).HasColumnName("saldo");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("pago");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idpago").ValueGeneratedOnAdd();
            entity.Property(e => e.Monto_Pagado).HasColumnName("monto");
            entity.Property(e => e.Pagado).HasColumnName("pagado");
            entity.Property(e => e.Id_Chofer).HasColumnName("idChofer");
            entity.Property(e => e.Id_Viaje).HasColumnName("idViaje");
            entity.Property(e => e.Id_sueldo).HasColumnName("idSueldo");
        });
        modelBuilder.Entity<Sueldo>(entity =>
        {
            entity.ToTable("sueldo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idsueldo").ValueGeneratedOnAdd();
            entity.Property(e => e.Monto).HasColumnName("monto_total");
            entity.Property(e => e.Id_Chofer).HasColumnName("idchofer");
            entity.Property(e => e.pagadoDesde).HasColumnName("fecha_desde");
            entity.Property(e => e.pagadoHasta).HasColumnName("fecha_hasta");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.Pagado).HasColumnName("Pagado");
            entity.Property(e => e.IdCamion).HasColumnName("idCamion");


        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("cliente");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idCliente");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasMany(c => c.Viajes)
                .WithOne(v => v.ClienteNavigation)
                .HasForeignKey(v => v.Cliente)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ViajeFlete>(entity =>
        {
            entity.ToTable("viaje_flete");
            entity.HasKey(e => e.idViajeFlete);
            entity.Property(e => e.idViajeFlete).HasColumnName("idviaje_flete");
            entity.Property(e => e.origen).HasColumnName("origen");
            entity.Property(e => e.destino).HasColumnName("destino");
            entity.Property(e => e.remito).HasColumnName("remito");
            entity.Property(e => e.carga).HasColumnName("carga");
            entity.Property(e => e.km).HasColumnName("km");
            entity.Property(e => e.kg).HasColumnName("kg");
            entity.Property(e => e.tarifa).HasColumnName("tarifa");
            entity.Property(e => e.factura).HasColumnName("factura");
            entity.Property(e => e.idCliente).HasColumnName("idCliente");
            entity.Property(e => e.idFlete).HasColumnName("fletero");
            entity.Property(e => e.nombre_chofer).HasColumnName("nombre_chofer");
            entity.Property(e => e.comision).HasColumnName("comision");
            entity.Property(e => e.fecha_salida).HasColumnName("fecha_salida");
        });

        modelBuilder.Entity<Flete>(entity =>
        {
            entity.ToTable("fletero");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idFletero");
            entity.Property(e => e.nombre).HasColumnName("nombre");
        });


        modelBuilder.Entity<Chofer>(entity =>
        {
            entity.ToTable("chofer");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idChofer");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        // Agregamos esto al método OnModelCreating en ApplicationDbContext.cs

        modelBuilder.Entity<Cheque>(entity =>
        {
            entity.ToTable("cheque");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcheque").ValueGeneratedOnAdd();
            entity.Property(e => e.FechaIngresoCheque).HasColumnName("fecha_ingreso");
            entity.Property(e => e.NumeroCheque).HasColumnName("nro_cheque");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.Banco).HasColumnName("banco");
            entity.Property(e => e.FechaCobro).HasColumnName("fecha_cobro");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.NumeroPersonalizado).HasColumnName("numero_personalizado");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimieto");
            entity.Property(e => e.EntregadoA).HasColumnName("entregado_a"); // NUEVA CONFIGURACIÓN - Más necesaria que un tutorial de citas para programadores
        });

        base.OnModelCreating(modelBuilder);
    }
}

