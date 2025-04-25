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

            PagoViewModel pw = new PagoViewModel();
           




            //float suel=  await pw.ObtenerSueldoCalculado(1, DateOnly.MinValue, DateOnly.MaxValue);


            SueldoViewModel sueldoViewModel = new SueldoViewModel();
            await sueldoViewModel.InsertarSueldo(1, DateOnly.MinValue, DateOnly.MaxValue);
          //  Console.WriteLine("sueldo calculado : " + suel);

            //SueldoViewModel sw = new SueldoViewModel();
            //sw.InsertarSueldo(1,DateOnly.Parse("2025/7/3"), DateOnly.Parse("2025/7/3"), DateOnly.MaxValue).Wait();



















            //PRUEBA PAGOS
            //PagoRepository pr = new PagoRepository(General.obtenerInstancia());
            //PagosService pagosService = new PagosService(pr);

            //pagosService.Crear(1, DateOnly.MinValue, DateOnly.MaxValue, DateOnly.MaxValue);

            //CamionViewModel cvm = new CamionViewModel();
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


           // CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();

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


            //ClienteViewModel clvm = new ClienteViewModel();

            //INSERCION
            //var cliente = await clvm.InsertarCliente("MACHACA");
            //if (cliente.IsSuccess)
            //{
            //    Console.WriteLine("Cliente insertado con el id: " + cliente.Value);
            //}

            //OBTENER VIAJES DE UN CLIENTE

            //var viajes = clvm.ObtenerViajesDeUnCliente("cooperativa");
            //if (viajes.Result.IsSuccess)
            //{
            //    foreach(var viaje in viajes.Result.Value)
            //    {
            //        Console.WriteLine(viaje.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(viajes.Result.Error);
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

                //ViajeFleteViewModel vfvm = new ViajeFleteViewModel();

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

                //Console.WriteLine(1);
                //ProbarInsertarViaje("Tandil", "Azul");

               //try
               //{
               // PRUEBAS CHOFER
                    //await ProbarInsertarChofer("McLovin");
                    //await ProbarObtenerChoferPorId(1);
                    //await ProbarObtenerTodosChoferes();
                    //await ProbarActualizarChofer(1, "McLovin Actualizado");
                    //await ProbarEliminarChofer(1);

                //    await ProbarInsertarViaje("Tandil", "Miami");

                //    await ProbarObtenerViajePorId(1);
                //    await ProbarObtenerTodosViajes();
                //    await ProbarObtenerViajesPorCamion(3);
                //    await ProbarActualizarViaje(1, destino: "Las Vegas");
                //    await ProbarEliminarViaje(1);

                //    Console.WriteLine("¡Todas las pruebas completadas!");
                //}
        //       catch (Exception ex)
        //       {
        //          Console.WriteLine($"¡ERROR CRÍTICO! {ex.Message}");
        //          Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        //          if (ex.InnerException != null)
        //              Console.WriteLine($"Error interno: {ex.InnerException.Message}");
        //      }
        }


        public static async Task<ViajeDTO> ProbarInsertarViaje(string origen, string destino)
        {
            Console.WriteLine(2);

            ViajeViewModel vvm = new ViajeViewModel();

            var resultadoCreacion1 = await vvm.CrearAsync(
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
                return resultadoCreacion1.Value;
            }

            Console.WriteLine("Resultado de la creación del viaje: " + resultadoCreacion1.Value);
            return new ViajeDTO();
        }


        private static async Task ProbarInsertarChofer(string nombre, string descripcion = null)
        {
            Console.WriteLine($"\n=== INSERTANDO CHOFER: {nombre} ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultadoCreacion = await cvm.CrearAsync(nombre);
                if (resultadoCreacion.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Chofer creado: {resultadoCreacion.Value.Nombre} - ID generado o asignado por la DB");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo crear el chofer: {resultadoCreacion.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al crear chofer: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
            }
        }

        private static async Task ProbarObtenerChoferPorId(int id)
        {
            Console.WriteLine($"\n=== OBTENIENDO CHOFER ID: {id} ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultado = await cvm.ObtenerPorIdAsync(id);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Chofer encontrado: {resultado.Value.Nombre}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se encontró el chofer: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener chofer: {ex.Message}");
            }
        }

        private static async Task ProbarObtenerTodosChoferes()
        {
            Console.WriteLine("\n=== OBTENIENDO TODOS LOS CHOFERES ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultado = await cvm.ObtenerTodosAsync();
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Choferes encontrados: {resultado.Value.Count}");
                    foreach (var chofer in resultado.Value)
                    {
                        Console.WriteLine($"  - {chofer.Nombre}");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudieron obtener los choferes: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener todos los choferes: {ex.Message}");
            }
        }

        private static async Task ProbarActualizarChofer(int id, string nuevoNombre)
        {
            Console.WriteLine($"\n=== ACTUALIZANDO CHOFER ID {id} A NOMBRE: {nuevoNombre} ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultado = await cvm.ActualizarAsync(id, nuevoNombre);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Chofer actualizado: {resultado.Value.Nombre}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo actualizar el chofer: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al actualizar chofer: {ex.Message}");
            }
        }

        private static async Task ProbarEliminarChofer(int id)
        {
            Console.WriteLine($"\n=== ELIMINANDO CHOFER ID: {id} ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultado = await cvm.EliminarAsync(id);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Chofer eliminado correctamente: {resultado.Value}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo eliminar el chofer: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al eliminar chofer: {ex.Message}");
            }
        }

        // MÉTODOS DE PRUEBA PARA VIAJE

        private static async Task ProbarObtenerViajePorId(int id)
        {
            Console.WriteLine($"\n=== OBTENIENDO VIAJE ID: {id} ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                // Este método no existe en tu ViajeViewModel, deberías agregarlo
                // Este es solo un ejemplo de cómo debería ser
                var resultado = await vvm.ObtenerPorIdAsync(id);
                if (resultado.IsSuccess)
                {
                    var viaje = resultado.Value;
                    Console.WriteLine($"[ÉXITO] Viaje encontrado: {viaje.LugarPartida} → {viaje.Destino}");
                    Console.WriteLine($"  Cliente: {viaje.NombreCliente}, Chofer: {viaje.NombreChofer}");
                    Console.WriteLine($"  Precio total: ${viaje.PrecioViaje:F2}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se encontró el viaje: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener viaje: {ex.Message}");
            }
        }

        private static async Task ProbarObtenerTodosViajes()
        {
            Console.WriteLine("\n=== OBTENIENDO TODOS LOS VIAJES ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                var resultado = await vvm.ObtenerTodosAsync();
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.PrecioViaje:F2})");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudieron obtener los viajes: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener todos los viajes: {ex.Message}");
            }
        }

        private static async Task ProbarObtenerViajesPorCamion(int camionId)
        {
            Console.WriteLine($"\n=== OBTENIENDO VIAJES POR CAMIÓN ID: {camionId} ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                var resultado = await vvm.ObtenerPorCamionAsync(camionId);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados para el camión {camionId}: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.PrecioViaje:F2})");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudieron obtener los viajes: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener viajes por camión: {ex.Message}");
            }
        }

        private static async Task ProbarActualizarViaje(
            int id,
            DateOnly? fechaInicio = null,
            string lugarPartida = null,
            string destino = null,
            int? remito = null,
            string carga = null,
            float? kg = null,
            int? cliente = null,
            int? camion = null,
            float? km = null,
            float? tarifa = null)
        {
            Console.WriteLine($"\n=== ACTUALIZANDO VIAJE ID: {id} ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                var resultado = await vvm.ActualizarAsync(
                    id, fechaInicio, lugarPartida, destino,
                    remito, carga, kg, cliente, camion, km, tarifa);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viaje actualizado correctamente");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo actualizar el viaje: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al actualizar viaje: {ex.Message}");
            }
        }

        private static async Task ProbarEliminarViaje(int id)
        {
            Console.WriteLine($"\n=== ELIMINANDO VIAJE ID: {id} ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                var resultado = await vvm.EliminarAsync(id);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viaje eliminado correctamente: {resultado.Value}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo eliminar el viaje: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al eliminar viaje: {ex.Message}");
            }
        }
    }
}

