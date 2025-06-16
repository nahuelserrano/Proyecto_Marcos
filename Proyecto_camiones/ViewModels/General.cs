using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto_camiones.Presentacion;
using System;

namespace Proyecto_camiones.ViewModels
{
    public static class General
    {
        public static ApplicationDbContext obtenerInstancia()
        {
            var connectionString = "server=metro.proxy.rlwy.net;port=11105;database=railway;user=root;password=OiFfLgfCAGtvTcAhfckpmJlSlJsEIvZY;";

            // Crear la configuración del DbContextAdd commentMore actions
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                //.EnableSensitiveDataLogging()
                //.LogTo(Console.WriteLine, LogLevel.Information) // Usar el proveedor de MySQL
                //.EnableDetailedErrors()
                ;
            // Crear una instancia del contexto
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }

    }
}
