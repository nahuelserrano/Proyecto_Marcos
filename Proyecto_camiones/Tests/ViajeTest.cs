using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Tests
{
    public static class ViajeTests
    {
        public static async Task EjecutarTodasLasPruebas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE VIAJE =======\n");

            try
            {
                DateOnly fecha = new DateOnly(2025, 4, 28);
                int id = await ProbarInsertarViaje(fecha, "Tandil", "Buenos Aires", 123, 1000.5f, "Trigo", "Cliente1", "HIJ429", 350.5f, 5000.0f, "Chofer Test", 0.18f);
                await ProbarObtenerViajePorId(id);
                await ProbarObtenerTodosViajes();
                await ProbarObtenerViajesPorCamion(3);
                await ProbarObtenerViajesPorCliente(3);
                await ProbarObtenerViajesPorChofer("Pepito");
                await ProbarActualizarViaje(id, destino: "Las Vegas");
                await ProbarEliminarViaje(id);

                Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE VIAJE =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas de viaje: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public static async Task<int> ProbarInsertarViaje(DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            float kg,
            string carga,
            string cliente,
            string camion,
            float km,
            float tarifa,
            string nombreChofer, 
            float porcentajeChofer)
        {
            ViajeViewModel vvm = new ViajeViewModel();

            var resultadoCreacion1 = await vvm.CrearAsync(
                fechaInicio: fechaInicio,
                lugarPartida: lugarPartida,
                destino: destino,
                remito: remito,
                carga: carga,
                kg: kg,
                cliente: cliente, // Asumiendo que el ID 2 existe
                camion: camion, // Asumiendo que el ID 3 existe
                km: km,
                tarifa: tarifa,
                nombreChofer: nombreChofer,
                porcentajeChofer: porcentajeChofer// Asumiendo que el chofer existe
            );

            if (!resultadoCreacion1.IsSuccess)
            {
                Console.WriteLine("Error al crear el viaje: " + resultadoCreacion1.Error);
                return -1;
            }

            Console.WriteLine("Resultado de la creación del viaje: " + resultadoCreacion1.Value);
            return resultadoCreacion1.Value;
        }

        public static async Task ProbarObtenerViajePorId(int id)
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
                    Console.WriteLine(viaje.ToString());
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

        public static async Task ProbarObtenerTodosViajes()
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

        public static async Task ProbarObtenerViajesPorCamion(int camionId)
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

        public static async Task ProbarObtenerViajesPorChofer(int id)
        {
            try
            {
                Console.WriteLine($"\n=== OBTENIENDO VIAJE POR CHOFER CON ID: {id} ===");
                ViajeViewModel vvm = new ViajeViewModel();

                var resultado = await vvm.ObtenerPorChoferAsync(id);
                Console.WriteLine("Superamos");

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        //Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.PrecioViaje:F2})");
                        Console.WriteLine(viaje.ToString());
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

        public static async Task ProbarObtenerViajesPorChofer(string nombre)
        {
            try
            {
                Console.WriteLine($"\n=== OBTENIENDO VIAJE POR CHOFER CON NOMBRE: {nombre} ===");
                ViajeViewModel vvm = new ViajeViewModel();

                var resultado = await vvm.ObtenerPorChoferAsync(nombre);
                //Console.WriteLine("Superamos");

                //var viajeRepository = new ViajeRepository(General.obtenerInstancia());

                //var prueba = await viajeRepository.ObtenerPorChoferAsync(nombre);

                //foreach (var VARIABLE in prueba)
                //{
                //    Console.WriteLine(VARIABLE);
                //}

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        //Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.PrecioViaje:F2})");
                        Console.WriteLine(viaje.ToString());
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

        public static async Task ProbarObtenerViajesPorCliente(int id)
        {
            try
            {
                Console.WriteLine($"\n=== OBTENIENDO VIAJE POR CLIENTE CON ID: {id} ===");
                ViajeViewModel vvm = new ViajeViewModel();

                var resultado = await vvm.ObtenerPorClienteAsync(id);
                Console.WriteLine("Superamos");

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        Console.WriteLine($"  - {viaje.Fecha_salida}: {viaje.Origen} → {viaje.Destino} (${viaje.Tarifa:F2})");
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

        public static async Task ProbarActualizarViaje(
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
            float? tarifa = null,
            string nombreChofer = null)
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

        public static async Task ProbarEliminarViaje(int id)
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
