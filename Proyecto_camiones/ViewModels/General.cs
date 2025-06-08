using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Presentacion;

namespace Proyecto_camiones.ViewModels
{
    public static class General
    {

        public static ApplicationDbContext obtenerInstancia()
        {
            var connectionString = "server=localhost;user=root;password=;database=truck_manager_project_db;";

            // Crear la configuración del DbContext
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
