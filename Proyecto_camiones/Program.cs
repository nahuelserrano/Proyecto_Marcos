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
using Proyecto_camiones.Presentacion.Models;
using System.Collections.Generic;


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
            //PagoViewModel pw = new PagoViewModel();
            //float suel=  await pw.ObtenerSueldoCalculado(1, DateOnly.MinValue, DateOnly.MaxValue);

            //SueldoViewModel sueldoViewModel = new SueldoViewModel();
            //await sueldoViewModel.InsertarSueldo(1, DateOnly.MinValue, DateOnly.MaxValue);
          //  Console.WriteLine("sueldo calculado : " + suel);


            //SueldoViewModel sw = new SueldoViewModel();
            //sw.InsertarSueldo(1,DateOnly.Parse("2025/7/3"), DateOnly.Parse("2025/7/3"), DateOnly.MaxValue).Wait();

            //PRUEBA PAGOS
            //PagoRepository pr = new PagoRepository(General.obtenerInstancia());
            //PagosService pagosService = new PagosService(pr);

            //pagosService.Crear(1, DateOnly.MinValue, DateOnly.MaxValue, DateOnly.MaxValue);

            CamionViewModel cvm = new CamionViewModel();
            ////PRUEBA INSERCION
            //Result<int> id = cvm.InsertarCamion("NCS234", "Mili").Result;
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
            //var camionUpdated = await cvm.Actualizar(11, "PUC111", "JUAN");
            //if (camionUpdated.IsSuccess)
            //{
            //    CamionDTO camion = camionUpdated.Value;
            //    Console.WriteLine("camion actualizado a: " + camion.ToString());
            //}

            //PRUEBA ELIMINAR CAMION
            //var response = await cvm.Eliminar(12);
            //Console.WriteLine(response.Value);
            //Console.WriteLine(response.Error);


            // CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

            //CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

            //INSERCION PARA CUENTA CORRIENTE DE UN FLETERO FUNCIONANDO CORRECTAMENTE

            //var cuenta = await ccvm.Insertar(null, "carlos", new DateOnly(2025, 4, 21), 90, 1000, 600);
            //if (cuenta.IsSuccess)
            //{
            //    Console.WriteLine("Id insertado: " + cuenta.Value);
            //}
            //Console.WriteLine(cuenta.Value);

            //INSERCION PARA CUENTA CORRIENTE DE UN CLIENTE FUNCIONANDO CORRECTAMENTE

            //var cuenta = await ccvm.Insertar("machaca", null, new DateOnly(2025, 4, 22), 92, 1000, 600);
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


            //OBTENER CUENTAS DE UN CLIENTE POR SU NOMBRE
            //var cuentasCliente5 = await ccvm.ObtenerCuentasByCliente("machaca");
            //if (cuentasCliente5.IsSuccess)
            //{
            //    foreach (CuentaCorrienteDTO c in cuentasCliente5.Value)
            //    {
            //        Console.WriteLine(c);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(cuentasCliente5.Error);
            //}

            //OBTENER LAS CUENTAS DE UN FLETERO POR SU NOMBRE


            //var cuentasFleteCarlos = await ccvm.ObtenerCuentasByFletero("carlos");
            //if (cuentasFleteCarlos.IsSuccess)
            //{
            //    foreach (CuentaCorrienteDTO c in cuentasFleteCarlos.Value)
            //    {
            //        Console.WriteLine(c);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(cuentasFleteCarlos.Error);
            //}

            //ELIMINAR 
            //var result = await clvm.Eliminar(1);
            //Console.WriteLine(result.Value);

            //ViajeFleteViewModel vfvm = new ViajeFleteViewModel();

            //INSERTAR

            //var idViaje = await vfvm.InsertarViajeFlete("Tandil", "Necochea", 40, "trigo", 120, 130, 19000, 12345, "MACHACA", "CARLOS", "Chofer de Carlos", 10, new DateOnly(2025, 4, 11));
            //if (idViaje.IsSuccess)
            //{
            //    Console.WriteLine("Viaje ingresado con el id: " + idViaje.Value);
            //}
            //Console.WriteLine(idViaje.Error);


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

            FleteViewModel fvm = new FleteViewModel();

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

            //OBTENER TODOS
            //var fleteros = await fvm.ObtenerTodosAsync();
            //if (fleteros.IsSuccess)
            //{
            //    List<Flete> fletes = fleteros.Value;
            //    foreach(Flete f in fletes)
            //    {
            //        Console.WriteLine(f.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(fleteros.Error);
            //}

            //try
            //{

            //    // Descomentar la prueba que se desea ejecutar

            //    // PRUEBAS DE CLIENTE
            //    //await ClienteTests.EjecutarTodasLasPruebas();

            //    // PRUEBAS DE CHOFER
            //    await ChoferTests.EjecutarTodasLasPruebas();

            //    // PRUEBAS DE VIAJE
            //    //await ViajeTests.EjecutarTodasLasPruebas();

            //    // O EJECUTAR PRUEBAS INDIVIDUALES:

            //    // CLIENTE
            //    //int idCliente await ClienteTests.ProbarInsertarCliente("TRANSPORTES TEST");
            //    //await ClienteTests.ProbarObtenerClientePorId(idCliente);
            //    //await ClienteTests.ProbarEliminarCliente(idCliente);

            //    // CHOFER
            //    //await ChoferTests.ProbarInsertarChofer("Chofer Test");
            //    //await ChoferTests.ProbarObtenerChoferPorId(1);
            //    //await ChoferTests.ProbarEliminarChofer(2);

            //    // VIAJE
            //    //DateOnly fecha = new DateOnly(2025, 4, 28);
            //    //int idViaje = await ViajeTests.ProbarInsertarViaje(fecha, "Tandil", "Buenos Aires", 123, 1000.5f, "Trigo", 1, 1, 350.5f, 5000.0f, "Chofer Test");
            //    //await ViajeTests.ProbarObtenerViajePorId(idViaje);
            //    //await ViajeTests.ProbarEliminarViaje(idViaje);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"\n¡ERROR CRÍTICO EN EL PROGRAMA DE PRUEBAS!");
            //    Console.WriteLine($"Mensaje: {ex.Message}");
            //    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            //}

            ClienteViewModel clientevm = new ClienteViewModel();
            //var response = await clientevm.ObtenerTodosAsync();
            //if (response.IsSuccess)
            //{
            //    foreach(Cliente c in response.Value)
            //    {
            //        Console.WriteLine(c.ToString());
            //    }
            //}

            //OBTENER TODOS LOS VIAJES DE UN CLIENTE
            //var viajes = await clientevm.ObtenerViajesDeUnCliente("cooperativa");
            //if (viajes.IsSuccess)
            //{
            //    foreach(ViajeMixtoDTO viaje in viajes.Value)
            //    {
            //        Console.WriteLine(viaje.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(viajes.Error);
            //}
        }
    }
}

