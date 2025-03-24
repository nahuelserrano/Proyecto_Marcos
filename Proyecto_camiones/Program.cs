using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Threading.Tasks;


namespace Proyecto_camiones.Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)

        {
            var connectionString = "server=localhost;user=root;password=;database=truck_manager_project;";
            Console.WriteLine("hola?");

            // Crear la configuración del DbContext
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); // Usar el proveedor de MySQL
            Console.WriteLine("superamos esto?");

            // Crear una instancia del contexto
            using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            Console.WriteLine("posible error1");
            // Usar el contexto en tu repositorio
            var camionRepository = new CamionRepository(dbContext);
            Console.WriteLine("posible error 2");
            Task<bool> result = camionRepository.ProbarConexionAsync();
            Console.WriteLine("pasé la prueba1");

            //ViajeRepository ReposViaje = new Repositories.ViajeRepository();
            //ChoferRepository ReposChofer = new Repositories.ChoferRepository();
            CamionService ServCamion = new(camionRepository);
            Console.WriteLine("holu???");
            //ChoferService ServChofer = new Services.ChoferService(ReposChofer);
            //ViajeService ServViaje = new Services.ViajeService(ReposViaje, ServCamion, ServChofer);
            //Camion camion = new Camion(10000, 200, "AAX2000");
            //Chofer chofer = new Chofer("nahuel", "serrano");
            //Cliente cliente = new Cliente("manteca", "mantecoso", 12345678);
            //DateTime fecha = new DateTime(2025, 03, 20);
            //DateTime fechaEntrega = new DateTime(2025, 04, 20);

            if(result.Result == true)
            {
                Console.WriteLine("omg entré!!");
                var resultado = await ServCamion.CrearCamionAsync(150, 100, "WWW123");

                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idCamion = resultado.Value;
                    Console.WriteLine($"Camión creado con ID: {idCamion}");
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el camión: {resultado.Error}");
                }
            }



            //ViajeService viajeService = new ViajeService(ReposViaje, ServCamion, ServChofer);
            //Viaje viaje = new Viaje("olava", "tandil", 3, 334, 233, 344, chofer, cliente, camion, fecha, fechaEntrega, 88, camion.Id);


            //System.Console.WriteLine(viajeService.CrearViaje(viaje).Result.Value);

            //Console.WriteLine("estoy acá?");

            //// Obtener la instancia de la conexión
            //Conexion conexion = Conexion.getInstancia();

            ////// Probar la conexión
            //bool conexionExitosa = conexion.TestConexion();

            //if (conexionExitosa)
            //{
            //    Console.WriteLine("La conexión a la base de datos fue exitosa.");
            //}
            //else
            //{
            //    Console.WriteLine("No se pudo conectar a la base de datos.");
            //}

        }
    }
}
