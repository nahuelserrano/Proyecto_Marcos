using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;


namespace Proyecto_camiones.Presentacion
{
    static class Program
    {

        public static ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)

        {
            // ----------------------------- ESTABLECER CONEXION --------------------------

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

            CamionService ServCamion = new(camionRepository);
            Console.WriteLine("holu???");

            //-------------------------------------------INSERCION----------------------------------------

            //if(result.Result == true)
            //{
            //    Console.WriteLine("omg entré!!");
            //    var resultado = await ServCamion.CrearCamionAsync(150, 100, "WWW123");

            //    // Ahora puedes acceder al resultado
            //    if (resultado.IsSuccess)
            //    {
            //        // La operación fue exitosa
            //        int idCamion = resultado.Value;
            //        Console.WriteLine($"Camión creado con ID: {idCamion}");
            //    }
            //    else
            //    {
            //        // Si la operación falló, maneja el error
            //        Console.WriteLine($"Error al crear el camión: {resultado.Error}");
            //    }
            //}


            //---------------------------OBTENCION------------------------------------------

            if(result.Result == true)
            {
                var camiones = await ServCamion.ObtenerCamionesAsync();
                Console.WriteLine("no rompió ante la llamada");
                foreach(var camion in camiones){
                    Console.WriteLine(camion.ToString());
                }
            }
        }
    }
}
