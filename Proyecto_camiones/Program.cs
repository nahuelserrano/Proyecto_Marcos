using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.ViewModels;


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

            CamionViewModel cvm = new CamionViewModel();
            //PRUEBA INSERCION
            int id = cvm.InsertarCamion(220, 200, "hhh902").Result;
            Console.WriteLine("se pudo agregar con el id: " + id);

            //PRUEBA SELECT ALL
            var camiones = await cvm.ObtenerTodos();
            foreach (var camion in camiones)
            {
                Console.WriteLine(camion.ToString());
            }

        }
    }
}
