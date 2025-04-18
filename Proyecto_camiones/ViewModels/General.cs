using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Presentacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public static class General
    {

        private static ApplicationDbContext _instance;

        public static ApplicationDbContext obtenerInstancia()
        {
            if (_instance == null)
            {
                // Conexión directa que SABEMOS que funciona
                string connectionString = "server=localhost;database=truck_manager_project_db;user=root;password=;Connect Timeout=10;";

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseMySql(
                    connectionString,
                    // Usamos versión específica en lugar de AutoDetect
                    new MySqlServerVersion(new Version(10, 4, 32)), // Ajusta esto a tu versión de MySQL
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null);
                        mySqlOptions.CommandTimeout(15);
                    }
                );

                _instance = new ApplicationDbContext(optionsBuilder.Options);
            }

            return _instance;
        }
    }
}
