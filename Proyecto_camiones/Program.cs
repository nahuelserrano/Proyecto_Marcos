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
using Proyecto_camiones.Tests;
using System.Collections.Generic;
using System.Linq;
using Proyecto_camiones.Front;
using Mysqlx.Cursor;

namespace Proyecto_camiones
{
    static class Program
    {


        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)

        {

            // Llamada a Windows Forms para inicializar la aplicación
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Front.Viaje()); // Ejecuta el formulario principal
            //PagoViewModel pw = new PagoViewModel();

            //await pw.CrearAsync(1, 1, 1000);
            //await pw.CrearAsync(1, 1, 1000);
            //await pw.CrearAsync(1, 1, 1000);
            //await pw.CrearAsync(1, 1, 1000);


            //float suel=  await pw.ObtenerSueldoCalculado(1, DateOnly.MinValue, DateOnly.MaxValue);

            SueldoViewModel sueldoViewModel = new SueldoViewModel();
           //await sueldoViewModel.CrearAsync(1, DateOnly.MinValue, DateOnly.MaxValue);
          //  Console.WriteLine("sueldo calculado : " + suel);


            //SueldoViewModel sw = new SueldoViewModel();
            //sw.InsertarSueldo(1,DateOnly.Parse("2025/7/3"), DateOnly.Parse("2025/7/3"), DateOnly.MaxValue).Wait();

            //PRUEBA PAGOS
            //PagoRepository pr = new PagoRepository(General.obtenerInstancia());
            //PagosService pagosService = new PagosService(pr);

            //pagosService.Crear(1, DateOnly.MinValue, DateOnly.MaxValue, DateOnly.MaxValue);

            CamionViewModel cvm = new CamionViewModel();
            ////PRUEBA INSERCION
            //Result<int> id = cvm.InsertarAsync("NCS234", "Mili").Result;
            //if (id.IsSuccess)
            //{
            //    Console.WriteLine("se pudo agregar con el id: " + id.Value);
            //}
            //else
            //{
            //    Console.WriteLine(id.Error);

            //PRUEBA SELECT ALL
            //var camiones = await cvm.ObtenerTodosAsync();
            //if (camiones.IsSuccess)
            //{
            //    foreach (var camion in camiones.Value)
            //    {
            //        Console.WriteLine(camion.ToString());
            //    }
            //}

            //PRUEBA UPDATE CAMION
            //var camionUpdated = await cvm.ActualizarAsync(11, "PUC111", "JUAN");
            //if (camionUpdated.IsSuccess)
            //{
            //    CamionDTO camion = camionUpdated.Value;
            //    Console.WriteLine("camion actualizado a: " + camion.ToString());
            //}

            //PRUEBA ELIMINAR CAMION
            //var response = await cvm.EliminarAsync(2);
            //if (response.IsSuccess)
            //{
            //    Console.WriteLine(response.Value);
            //}
            //else
            //{
            //    Console.WriteLine(response.Error);
            //}

            CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

            //INSERCION PARA CUENTA CORRIENTE DE UN FLETERO FUNCIONANDO CORRECTAMENTE

            //var cuenta = await ccvm.InsertarAsync(null, "carlos", new DateOnly(2025, 4, 21), 90, 1000, 600);
            //if (cuenta.IsSuccess)
            //{
            //    Console.WriteLine("Id insertado: " + cuenta.Value);
            //}
            //else
            //{
            //    Console.WriteLine(cuenta.Error);
            //}

            //INSERCION PARA CUENTA CORRIENTE DE UN CLIENTE FUNCIONANDO CORRECTAMENTE

            //var cuenta = await ccvm.InsertarAsync("machaca", null, new DateOnly(2025, 5, 13), 15, 5000, 0);
            //if (cuenta.IsSuccess)
            //{
            //    Console.WriteLine("Id insertado: " + cuenta.Value);
            //}
            //else
            //{
            //    Console.WriteLine(cuenta.Error);
            //}


            //OBTENER CUENTAS DE UN CLIENTE POR SU NOMBRE
            //var cuentasCliente5 = await ccvm.ObtenerCuentasByClienteAsync("machaca");
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


            //var cuentasFleteCarlos = await ccvm.ObtenerCuentasByFleteroAsync("carlos");
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

            //ELIMINAR CUENTA CORRIENTE
            //var result = await ccvm.EliminarAsync(1);
            //if (result.IsSuccess)
            //{
            //    Console.WriteLine(result.Value);
            //}
            //else
            //{
            //    Console.WriteLine(result.Error);
            //}

            //ACTUALIZAR UNA CUENTA CORRIENTE
            //var cuenta = await ccvm.ActualizarAsync(16, null, 20, 1000, 0, null, null);
            //if (cuenta.IsSuccess)
            //{
            //    Console.WriteLine(cuenta.Value.ToString());
            //}
            //else
            //{
            //    Console.WriteLine(cuenta.Error);
            //}


            ViajeFleteViewModel vfvm = new ViajeFleteViewModel();

            //INSERTAR

            //var idViaje = await vfvm.InsertarAsync("Tandil", "Necochea", 40, "trigo", 120, 130, 19000, 12345, "MACHACA", "CARLOS", "Chofer de Carlos", 10, new DateOnly(2025, 4, 11));
            //if (idViaje.IsSuccess)
            //{
            //    Console.WriteLine("Viaje ingresado con el id: " + idViaje.Value);
            //}
            //Console.WriteLine(idViaje.Error);


            //OBTENER VIAJES DE UN FLETERO

            //var viajes = await vfvm.ObtenerViajesDeUnFleteroAsync("Carlos");
            //if (viajes.IsSuccess)
            //{
            //    foreach (var viaje in viajes.Value)
            //    {
            //        Console.WriteLine(viaje);
            //        Console.WriteLine("total: " + viaje.total + "comision: " + viaje.total_comision);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(viajes.Error);
            //}

            //ELIMINAR VIAJE POR FLETERO
            //var response = await vfvm.EliminarAsync(12);
            //if (response.IsSuccess)
            //{
            //    Console.WriteLine(response.Value);
            //}
            //else
            //{
            //    Console.WriteLine(response.Error);
            //}

            //OBTENER VIAJES POR FLETERO DE UN CLIENTE

            //var viajes = await vfvm.ObtenerViajesDeUnClienteAsync(3);
            //if (viajes.IsSuccess)
            //{
            //    foreach(var viaje in viajes.Value)
            //    {
            //        Console.WriteLine(viaje.ToString());
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
            //var idFletero = await fvm.InsertarAsync("Carlos");
            //if (idFletero.IsSuccess)
            //{
            //    Console.WriteLine("Fletero insertado con el id: " + idFletero.Value);
            //}
            //else
            //{
            //    Console.WriteLine(idFletero.Error);
            //}

            //OBTENER POR NOMBRE
            //var fletero = await fvm.ObtenerFletePorNombreAsync("Carlos");
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
            //    foreach (Flete f in fletes)
            //    {
            //        Console.WriteLine(f.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(fleteros.Error);
            //}

            //ELIMINAR FLETERO

            //var response = await fvm.EliminarAsync(1);
            //if (response.IsSuccess)
            //{
            //    Console.WriteLine(response.Value);
            //}
            //else
            //{
            //    Console.WriteLine(response.Error);
            //}

            //ACTUALIZAR FLETERO
            //var fletero = await fvm.ActualizarAsync(5, "marcos");
            //if (fletero.IsSuccess)
            //{
            //    Console.WriteLine(fletero.Value.ToString());
            //}
            //else
            //{
            //    Console.WriteLine(fletero.Error);
            //}

                //try
                //{

                //    // Descomentar la prueba que se desea ejecutar

                //    // PRUEBAS DE CLIENTE
                //    //await ClienteTests.EjecutarTodasLasPruebas();

                //    // PRUEBAS DE CHOFER
                //await ChoferTests.EjecutarTodasLasPruebas();

                //    // PRUEBAS DE VIAJE
                //await ViajeTest.EjecutarTodasLasPruebas();

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
                DateOnly fecha = new DateOnly(2025, 4, 28);
            //int idViaje = await ViajeTests.ProbarInsertarViaje(fecha, "Tandil", "Buenos Aires", 123, 1000.5f, "Trigo", "Cliente1", "HIJ429", 350.5f, 5000.0f, "Chofer Test", 018f);

            //await ViajeTest.ProbarActualizarViaje(
            //    id: 1,
            //    fechaInicio: new DateOnly(2025, 5, 10),
            //    lugarPartida: "Rosario",
            //    destino: "Córdoba",
            //    remito: 12345,
            //    carga: "Soja",
            //    kg: 25000.5f,
            //    cliente: "COOPERATIVA",
            //    camion: "HIJ429",
            //    km: 400.0f,
            //    tarifa: 75000.0f,
            //    nombreChofer: "Chofer Test"
            //);

            //await ViajeTest.ProbarObtenerViajePorId(4);
            //await ViajeTest.ProbarObtenerViajesPorCliente(3);
            //await ViajeTest.ProbarObtenerViajesPorChofer(2);
            //await ViajeTest.ProbarObtenerViajesDeUnFletero("Carlos");
            //await ViajeTest.ProbarEliminarViaje(10);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"\n¡ERROR CRÍTICO EN EL PROGRAMA DE PRUEBAS!");
            //    Console.WriteLine($"Mensaje: {ex.Message}");
            //    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            //}

            ClienteViewModel clientevm = new ClienteViewModel();

            //OBTENER TODOS LOS CLIENTES

            //var response = await clientevm.ObtenerTodosAsync();
            //if (response.IsSuccess)
            //{
            //    foreach (Cliente c in response.Value)
            //    {
            //        Console.WriteLine(c.ToString());
            //    }
            //}

            //OBTENER TODOS LOS VIAJES DE UN CLIENTE
            //var viajes = await clientevm.ObtenerViajesDeUnClienteAsync("cooperativa");
            //if (viajes.IsSuccess)
            //{
            //    foreach (ViajeMixtoDTO viaje in viajes.Value)
            //    {
            //        Console.WriteLine(viaje.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(viajes.Error);
            //}

            //await ChequeTests.EjecutarTodasLasPruebas();

            //ELIMINAR CLIENTE

            //var response = await clientevm.EliminarAsync(3);
            //if (response.IsSuccess)
            //{
            //    Console.WriteLine(response.Value);
            //}
            //else
            //{
            //    Console.WriteLine(response.Error);
            //}

            //ACTUALIZAR CLIENTE
            //var cliente = await clientevm.ActualizarAsync(2, "Verellen");
            //if (cliente.IsSuccess)
            //{
            //    Console.WriteLine(cliente.Value.ToString());
            //}
            //else
            //{
            //    Console.WriteLine(cliente.Error);
            //}
        }
    }
}

