using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto_camiones.Presentacion;
using System;

namespace Proyecto_camiones.ViewModels
{
    public static class General
    {
        private static ApplicationDbContext _instancia;
        private static readonly object _lock = new object();

        public static ApplicationDbContext obtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        string connectionString = "Server=truck-managment-logistics-truck-manager-logistics-project.l.aivencloud.com;Port=19680;Database=defaultdb;Uid=avnadmin;Pwd=AVNS_sr1_CB5BquO8hPsHHYC;SslMode=Required;Pooling=true;MinimumPoolSize=5;MaximumPoolSize=100;ConnectionTimeout=30;";

                        // Crear la configuración del DbContext
                        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                        // Crear una instancia del contexto
                        _instancia = new ApplicationDbContext(optionsBuilder.Options);
                    }
                }
            }
            return _instancia;
        }

        // Método para cerrar la conexión al cerrar la aplicación
        public static void cerrarInstancia()
        {
            _instancia?.Dispose();
            _instancia = null;
        }

        // Método para obtener una nueva instancia temporal (para operaciones que requieren tracking)
        public static ApplicationDbContext obtenerInstanciaTemporal()
        {
            string connectionString = "Server=truck-managment-logistics-truck-manager-logistics-project.l.aivencloud.com;Port=19680;Database=defaultdb;Uid=avnadmin;Pwd=AVNS_sr1_CB5BquO8hPsHHYC;SslMode=Required;Pooling=true;MinimumPoolSize=5;MaximumPoolSize=100;ConnectionTimeout=30;";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
