using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;


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
            //Result<int> id = cvm.InsertarCamion(180, 200, "PQJ123").Result;
            //if (id.IsSuccess)
            //{
            //    Console.WriteLine("se pudo agregar con el id: " + id.Value);
            //}
            //else
            //{
            //    Console.WriteLine(id.Error);
            //}


            //PRUEBA SELECT ALL
            //var camiones = await cvm.ObtenerTodos();
            //if (camiones.IsSuccess)
            //{
            //    foreach (var camion in camiones.Value)
            //    {
            //        Console.WriteLine(camion.ToString());
            //    }
            //}

            //PRUEBA UPDATE CAMION
            //var camionUpdated = await cvm.Actualizar(2, 100, null, "HIJ429");
            //if (camionUpdated.IsSuccess)
            //{
            //    CamionDTO camion = camionUpdated.Value;
            //    Console.WriteLine("camion actualizado a: " + camion.ToString());
            //}

            //PRUEBA ELIMINAR CAMION
            //var response = await cvm.Eliminar(8);
            //Console.WriteLine(response.Value);

            CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

            //INSERCION

            var cuenta = await ccvm.Insertar(2, new DateOnly(2025, 4, 7), 3333, 2345, 2344);
            if (cuenta.IsSuccess)
            {
                Console.WriteLine("Id insertado: " + cuenta.Value);
            }
            Console.WriteLine(cuenta.Value);

        }
    }
}
