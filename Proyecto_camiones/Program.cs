using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;


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
            //Result<int> id = cvm.InsertarCamion(200, 20, "HJK092").Result;
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

<<<<<<< HEAD
            EmpleadoViewModel evm = new EmpleadoViewModel();
            //PRUEBA INSERCION
            Result<int> id = evm.InsertarEmpleado("Juan").Result;
            if (id.IsSuccess)
            {
                Console.WriteLine("se pudo agregar con el id: " + id.Value);
            }
            else
            {
                Console.WriteLine(id.Error);
            }
            //PRUEBA SELECT ALL
=======
            CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

            //INSERCION

            //var cuenta = await ccvm.Insertar(5, new DateOnly(2025, 4, 7), 89, 5678, 899);
            //if (cuenta.IsSuccess)
            //{
            //    Console.WriteLine("Id insertado: " + cuenta.Value);
            //}
            //Console.WriteLine(cuenta.Value);

            //var cuenta2 = await ccvm.Insertar(5, new DateOnly(2025, 4, 7), 8383, 99, 22);
            //if (cuenta2.IsSuccess)
            //{
            //    Console.WriteLine("Id insertado: " + cuenta2.Value);
            //}
            //Console.WriteLine(cuenta2.Value);


            //OBTENER CUENTAS DE UN CLIENTE
            //var cuentasCliente5 = await ccvm.ObtenerCuentasByClienteId(5);
            //if (cuentasCliente5.IsSuccess)
            //{
            //    foreach(CuentaCorriente c in cuentasCliente5.Value)
            //    {
            //        Console.WriteLine(c);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(cuentasCliente5.Error);
            //}


             ClienteViewModel clvm = new ClienteViewModel();

            //INSERCION
            //var cliente = await clvm.InsertarCliente("MACHACA");
            //if (cliente.IsSuccess)
            //{
            //    Console.WriteLine("Cliente insertado con el id: " + cliente.Value);
            //}


            //OBTENER BY ID
            //var cliente = await clvm.ObtenerById(4);
            //if (cliente.IsSuccess)
            //{
            //    Console.WriteLine(cliente.Value);
            //}


            //ELIMINAR 
            //var result = await clvm.Eliminar(1);
            //Console.WriteLine(result.Value);



>>>>>>> d5a916ea94732eaeeb855405b77552168c389cd3
        }
    }
}
