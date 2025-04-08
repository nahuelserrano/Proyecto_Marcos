﻿using Microsoft.EntityFrameworkCore;
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
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<CuentaCorriente> Cuentas { get; set; }

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
        modelBuilder.Entity<Empleado>().ToTable("empleado");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Usuario>().ToTable("usuario");
        modelBuilder.Entity<CuentaCorriente>().ToTable("cuenta_corriente");

        modelBuilder.Entity<Camion>(entity =>
        {
            entity.ToTable("camion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcamion");
            entity.Property(e => e.peso_max).HasColumnName("peso_max");
            entity.Property(e => e.tara).HasColumnName("tara");
            entity.Property(e => e.Patente).HasColumnName("patente");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("empleado");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idempleado");
            entity.Property(e => e.nombre).HasColumnName("nombre");
            entity.Property(e => e.apellido).HasColumnName("apellido");
            entity.Property(e => e.tipo_empleado).HasColumnName("tipo_empleado");
        });

        modelBuilder.Entity<CuentaCorriente>(entity =>
        {
            entity.ToTable("cuenta_corriente");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idcuenta_corriente");
            entity.Property(e => e.Nro_factura).HasColumnName("nro_factura");
            entity.Property(e => e.Fecha_factura).HasColumnName("fecha_factura");
            entity.Property(e => e.Adeuda).HasColumnName("importe");
            entity.Property(e => e.Pagado).HasColumnName("pagado");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("cliente");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idCliente");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Apellido).HasColumnName("apellido");
        })

        base.OnModelCreating(modelBuilder);

    }

}

