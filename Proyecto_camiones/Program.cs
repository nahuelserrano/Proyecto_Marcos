using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;


namespace Proyecto_camiones.Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)

        {
            var connectionString = "server=localhost;user=root;password=yourpassword;database=yourdatabase;";

            // Crear la configuración del DbContext
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySQL(connectionString); // Usar el proveedor de MySQL

            // Crear la instancia del DbContext
            var context = new ApplicationDbContext(optionsBuilder.Options);
            ViajeRepository ReposViaje = new Repositories.ViajeRepository();
            CamionRepository ReposCamion = new Repositories.CamionRepository(context);
            ChoferRepository ReposChofer = new Repositories.ChoferRepository();
            CamionService ServCamion = new Services.CamionService(ReposCamion);
            //ChoferService ServChofer = new Services.ChoferService(ReposChofer);
            //ViajeService ServViaje = new Services.ViajeService(ReposViaje, ServCamion, ServChofer);
            //Camion camion = new Camion(10000, 200, "AAX2000");
            //Chofer chofer = new Chofer("nahuel", "serrano");
            //Cliente cliente = new Cliente("manteca", "mantecoso", 12345678);
            //DateTime fecha = new DateTime(2025, 03, 20);
            //DateTime fechaEntrega = new DateTime(2025, 04, 20);


            //ViajeService viajeService = new ViajeService(ReposViaje, ServCamion, ServChofer);
            //Viaje viaje = new Viaje("olava", "tandil", 3, 334, 233, 344, chofer, cliente, camion, fecha, fechaEntrega, 88, camion.Id);


            //System.Console.WriteLine(viajeService.CrearViaje(viaje).Result.Value);

            Console.WriteLine("estoy acá?");

            // Obtener la instancia de la conexión
            Conexion conexion = Conexion.getInstancia();

            //// Probar la conexión
            bool conexionExitosa = conexion.TestConexion();

            if (conexionExitosa)
            {
                Console.WriteLine("La conexión a la base de datos fue exitosa.");
            }
            else
            {
                Console.WriteLine("No se pudo conectar a la base de datos.");
            }

        }
    }
}
