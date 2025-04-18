using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Runtime.CompilerServices;
using MySqlX.XDevAPI.Common;
using System.Data;
using MySql.Data.MySqlClient;


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

            //PRUEBA PAGOS
            //PagoRepository pr = new PagoRepository(General.obtenerInstancia());
            //PagosService pagosService = new PagosService(pr);

            //pagosService.Crear(1, DateOnly.MinValue, DateOnly.MaxValue, DateOnly.MaxValue);

            CamionViewModel cvm = new CamionViewModel();
            ////PRUEBA INSERCION
            //Result<int> id = cvm.InsertarCamion(120, 40, "MLA126", "Pepito").Result;
            //if (id.IsSuccess)
            //{
            //    Console.WriteLine("se pudo agregar con el id: " + id.Value);
            //}
            //else
            //{
            //    Console.WriteLine(id.Error);

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
            //var camionUpdated = await cvm.Actualizar(2, 100, null, "HIJ429", "JUAN");
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

            //var cuenta = await ccvm.Insertar(5, 1, new DateOnly(2025, 4, 7), 89, 5678, 899);
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

            ViajeFleteViewModel vfvm = new ViajeFleteViewModel();

            //INSERTAR

            //var idViaje = await vfvm.InsertarViajeFlete("Tandil", "Necochea", 40, "trigo", 120, 130, 19000, 12345, "MACHACA", "CARLOS", "Chofer de Carlos", 10, new DateOnly(2025, 4, 11));
            //if (idViaje.IsSuccess)
            //{
            //    Console.WriteLine("Viaje ingresado con el id: " + idViaje.Value);
            //}
            //Console.WriteLine(idViaje.Error);


            //ProbarInsertarChofer("Juan Alpaca");


            //OBTENER VIAJES DE UN FLETERO

            //var viajes = await vfvm.ObtenerViajesDeUnFletero("Carlos");
            //if (viajes.IsSuccess)
            //{
            //    foreach(var viaje in viajes.Value)
            //    {
            //        Console.WriteLine(viaje);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(viajes.Error);
            //}

            //ProbarInsertarViaje("Tandil", "Azul");

            //ProbarInsertarChofer("Juan Alpaca");

            //FleteViewModel fvm = new FleteViewModel();

            //INSERTAR FLETERO
            //var idFletero = await fvm.InsertarFletero("Carlos");
            //if (idFletero.IsSuccess)
            //{
            //    Console.WriteLine("Fletero insertado con el id: " + idFletero.Value);
            //}
            //else
            //{
            //    Console.WriteLine(idFletero.Error);
            //}

            //OBTENER POR NOMBRE
            //var fletero = await fvm.ObtenerFletePorNombre("Carlos");
            //if (fletero.IsSuccess)
            //{
            //    Console.WriteLine(fletero.Value);
            //}
            //else
            //{
            //    Console.WriteLine(fletero.Error);
            //}

            //ProbarEliminarChofer(2);

            Console.WriteLine(1);
            ProbarInsertarViaje("Tandil", "Azul");

            
        }


        public static async void ProbarInsertarViaje(string origen, string destino)
        {
            Console.WriteLine(2);

            ViajeViewModel vvm = new ViajeViewModel();

            var resultadoCreacion1 = await vvm.CrearViaje(
                fechaInicio: new DateOnly(2025, 4, 11),
                lugarPartida: origen,
                destino: destino,
                remito: 12345,
                carga: "Materiales de construcción",
                kg: 5000.5f,
                cliente: 2, // Asumiendo que el ID 2 existe
                camion: 3, // Asumiendo que el ID 3 existe
                km: 650.75f,
                tarifa: 10.5f
            );

            Console.WriteLine(3);

            if (!resultadoCreacion1.IsSuccess)
            {
                Console.WriteLine("Error al crear el viaje: " + resultadoCreacion1.Error);
                return;
            }

            Console.WriteLine("Resultado de la creación del viaje: " + resultadoCreacion1.Value);
        }

        public static void ProbarInsertarChofer(string nombre)
        {
            ChoferViewModel cvm = new ChoferViewModel();
            var resultadoCreacion = cvm.CrearAsync(nombre).Result;
            Console.WriteLine("Resultado de la creación del chofer: " + resultadoCreacion.Value.Nombre);
        }

        public static void ProbarEliminarChofer(int id)
        {
            ChoferViewModel cvm = new ChoferViewModel();
            var resultadoCreacion = cvm.EliminarAsync(id).Result;
            Console.WriteLine("Resultado de la creación del chofer: " + resultadoCreacion.Value);
        }
    }
}

