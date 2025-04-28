using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion
{
    public static class ChoferTests
    {
        public static async Task EjecutarTodasLasPruebas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE CHOFER =======\n");

            try
            {
                int id = await ProbarInsertarChofer("McLovin");
                await ProbarObtenerChoferPorId(id);
                await ProbarObtenerTodosChoferes();
                await ProbarActualizarChofer(id, "McLovin Actualizado");
                await ProbarEliminarChofer(id);

                Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE CHOFER =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas de chofer: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private static async Task<int> ProbarInsertarChofer(string nombre, string descripcion = null)
        {
            Console.WriteLine($"\n=== INSERTANDO CHOFER: {nombre} ===");
            ChoferViewModel cvm = new ChoferViewModel();

            try
            {
                var resultadoCreacion = await cvm.CrearAsync(nombre);
                if (resultadoCreacion.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Chofer creado: {resultadoCreacion.Value} - ID generado o asignado por la DB");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo crear el chofer: {resultadoCreacion.Error}");
                }
                return resultadoCreacion.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al crear chofer: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                return -1;
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
    }
}
