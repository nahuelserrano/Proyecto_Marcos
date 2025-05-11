using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Tests
{
    public static class ChequeTests
    {
        /// <summary>
        /// Ejecuta todas las pruebas de Cheque en secuencia
        /// </summary>
        public static async Task EjecutarTodasLasPruebas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE CHEQUE =======\n");

            try
            {
                // 1. Probamos la conexión primero
                await ProbarConexionCheque();

                // 2. Creamos un cheque para las pruebas
                int idCheque = await ProbarInsertarCheque(2, "12345ABC", 5000.0f, "Banco Galicia");

                // 3. Obtenemos el cheque por ID
                await ProbarObtenerChequePorId(idCheque);

                // 4. Probamos obtener todos los cheques
                await ProbarObtenerTodosCheques();

                // 5. Probamos actualizar un cheque
                await ProbarActualizarCheque(idCheque, banco: "Banco Nación", monto: 6000.0f);

                // 6. Eliminamos el cheque de prueba
                await ProbarEliminarCheque(idCheque);

                // 7. Verificamos si realmente se eliminó
                await ProbarObtenerChequePorId(idCheque);

                Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE CHEQUE =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas de cheque: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Prueba si se puede establecer la conexión con la base de datos desde ChequeViewModel
        /// </summary>
        public static async Task ProbarConexionCheque()
        {
            Console.WriteLine("\n=== PROBANDO CONEXIÓN DE CHEQUE ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                bool conexionExitosa = await chequeViewModel.TestearConexionAsync();

                if (conexionExitosa)
                {
                    Console.WriteLine("[ÉXITO] Conexión a la base de datos establecida correctamente");
                }
                else
                {
                    Console.WriteLine("[ERROR] No se pudo establecer conexión a la base de datos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al probar la conexión de Cheque: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la inserción de un nuevo cheque
        /// </summary>
        public static async Task<int> ProbarInsertarCheque(
            int idCliente,
            string numeroCheque,
            float monto,
            string banco)
        {
            DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Today);
            DateOnly fechaCobro = fechaHoy.AddDays(30); // Suponiendo un cheque a 30 días

            Console.WriteLine($"\n=== INSERTANDO CHEQUE: {numeroCheque} de ${monto} ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                var resultado = await chequeViewModel.CrearAsync(
                    idCliente: idCliente,
                    fechaIngreso: fechaHoy,
                    numeroCheque: numeroCheque,
                    monto: monto,
                    banco: banco,
                    fechaCobro: fechaCobro
                );

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cheque insertado con ID: {resultado.Value}");
                    return resultado.Value;
                }
                
                Console.WriteLine($"[ERROR] No se pudo insertar el cheque: {resultado.Error}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al insertar cheque: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Prueba la obtención de un cheque por su ID
        /// </summary>
        public static async Task ProbarObtenerChequePorId(int id)
        {
            Console.WriteLine($"\n=== OBTENIENDO CHEQUE POR ID: {id} ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                var resultado = await chequeViewModel.ObtenerPorIdAsync(id);

                if (resultado.IsSuccess)
                {
                    var cheque = resultado.Value;
                    //Console.WriteLine($"[ÉXITO] Cheque encontrado: ID {cheque.Id}");
                    Console.WriteLine($"  Número: {cheque.NumeroCheque}, Monto: ${cheque.Monto}");
                    Console.WriteLine($"  Banco: {cheque.Banco}, Fecha Cobro: {cheque.FechaCobro}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se encontró el cheque: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener cheque por ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la obtención de todos los cheques
        /// </summary>
        public static async Task ProbarObtenerTodosCheques()
        {
            Console.WriteLine("\n=== OBTENIENDO TODOS LOS CHEQUES ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                var resultado = await chequeViewModel.ObtenerTodosAsync();

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cheques encontrados: {resultado.Value.Count}");
                    foreach (var cheque in resultado.Value)
                    {
                        Console.WriteLine($"Número: {cheque.NumeroCheque}, Monto: ${cheque.Monto}, Banco: {cheque.Banco}");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudieron obtener los cheques: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener todos los cheques: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la actualización de un cheque
        /// </summary>
        public static async Task ProbarActualizarCheque(
            int id,
            int? idCliente = null,
            string numeroCheque = null,
            float? monto = null,
            string banco = null,
            DateOnly? fechaCobro = null)
        {
            Console.WriteLine($"\n=== ACTUALIZANDO CHEQUE ID: {id} ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                var resultado = await chequeViewModel.ActualizarAsync(
                    id: id,
                    idCliente: idCliente,
                    numeroCheque: numeroCheque,
                    monto: monto,
                    banco: banco,
                    fechaCobro: fechaCobro
                );

                if (resultado.IsSuccess)
                {
                    var cheque = resultado.Value;
                    Console.WriteLine($"[ÉXITO] Cheque actualizado correctamente");
                    Console.WriteLine($"  Nuevos valores: Número: {cheque.NumeroCheque}, Monto: ${cheque.Monto}");
                    Console.WriteLine($"  Banco: {cheque.Banco}, Fecha Cobro: {cheque.FechaCobro}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo actualizar el cheque: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al actualizar cheque: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la eliminación de un cheque
        /// </summary>
        public static async Task ProbarEliminarCheque(int id)
        {
            Console.WriteLine($"\n=== ELIMINANDO CHEQUE ID: {id} ===");

            try
            {
                var chequeViewModel = new ChequeViewModel();
                var resultado = await chequeViewModel.EliminarAsync(id);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Cheque eliminado correctamente: {resultado.Value}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo eliminar el cheque: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al eliminar cheque: {ex.Message}");
            }
        }
    }
}
