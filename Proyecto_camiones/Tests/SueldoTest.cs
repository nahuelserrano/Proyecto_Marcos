using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Tests
{
    public static class SueldoTest
    {
        /// <summary>
        /// Ejecuta todas las pruebas de Sueldo en secuencia
        /// </summary>
        public static async Task EjecutarTodasLasPruebas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE SUELDO =======\n");

            try
            {
                // 1. Probamos la conexión primero
                await ProbarConexionSueldo();

                // 2. Probamos las validaciones básicas
                await ProbarValidacionesSueldo();

                // 3. Creamos un sueldo para las pruebas
                int idSueldo = await ProbarCrearSueldo("Juan Pérez",
                    new DateOnly(2025, 5, 1),
                    new DateOnly(2025, 5, 31),
                    new DateOnly(2025, 5, 29),
                    "AAA111");

                // 4. Probamos obtener todos los sueldos con filtros
                await ProbarObtenerTodosSueldos("AAA111", "Juan Pérez");
                await ProbarObtenerTodosSueldos("AAA111", null);
                await ProbarObtenerTodosSueldos(null, "Juan Pérez");

                // 5. Probamos marcar como pagado
                await ProbarMarcarSueldoPagado(idSueldo);

                // 6. Eliminamos el sueldo de prueba
                await ProbarEliminarSueldo(idSueldo);

                // 7. Probamos el flujo completo
                await ProbarFlujoCompletoSueldo();

                Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE SUELDO =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas de sueldo: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Prueba si se puede establecer la conexión con la base de datos desde SueldoViewModel
        /// </summary>
        public static async Task ProbarConexionSueldo()
        {
            Console.WriteLine("\n=== PROBANDO CONEXIÓN DE SUELDO ===");

            try
            {
                var sueldoViewModel = new SueldoViewModel();
                bool conexionExitosa = await sueldoViewModel.testearConexion();

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
                Console.WriteLine($"[EXCEPCIÓN] Al probar la conexión de Sueldo: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la creación de un nuevo sueldo
        /// </summary>
        public static async Task<int> ProbarCrearSueldo(
            string nombreChofer,
            DateOnly fechaDesde,
            DateOnly fechaHasta,
            DateOnly? fechaPago = null,
            string patenteCamion = null)
        {
            Console.WriteLine($"\n=== CREANDO SUELDO PARA: {nombreChofer} ===");
            Console.WriteLine($"Período: {fechaDesde} → {fechaHasta}");
            Console.WriteLine($"Camión: {patenteCamion ?? "Sin especificar"}");

            try
            {
                var sueldoViewModel = new SueldoViewModel();
                var resultado = await sueldoViewModel.CrearAsync(
                    nombre_chofer: nombreChofer,
                    pagoDesde: fechaDesde,
                    pagoHasta: fechaHasta,
                    fechaPago: fechaPago,
                    patente_camion: patenteCamion
                );

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Sueldo creado con ID: {resultado.Value}");
                    return resultado.Value;
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo crear el sueldo: {resultado.Error}");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al crear sueldo: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Prueba la obtención de todos los sueldos con filtros
        /// </summary>
        public static async Task ProbarObtenerTodosSueldos(string patenteCamion = null, string nombreChofer = null)
        {
            Console.WriteLine($"\n=== OBTENIENDO TODOS LOS SUELDOS ===");
            Console.WriteLine($"Camión: {patenteCamion ?? "Todos"}");
            Console.WriteLine($"Chofer: {nombreChofer ?? "Todos"}");

            try
            {
                var sueldoViewModel = new SueldoViewModel();
                var resultado = await sueldoViewModel.ObtenerTodosAsync(patenteCamion, nombreChofer);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Sueldos encontrados: {resultado.Value.Count}");

                    foreach (var sueldo in resultado.Value)
                    {
                        string estado = sueldo.Pagado ? "PAGADO" : "PENDIENTE";
                        Console.WriteLine($"  ID: {sueldo.idSueldo}, Chofer: {sueldo.Id_Chofer}, Monto: ${sueldo.Monto_Pagado:F2}, Estado: {estado}");
                        Console.WriteLine($"  Período: {sueldo.PagadoDesde} → {sueldo.PagadoHasta}");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudieron obtener los sueldos: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al obtener todos los sueldos: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba marcar un sueldo como pagado
        /// </summary>
        public static async Task ProbarMarcarSueldoPagado(int id)
        {
            Console.WriteLine($"\n=== MARCANDO SUELDO COMO PAGADO: ID {id} ===");

            try
            {
                var sueldoViewModel = new SueldoViewModel();
                var resultado = await sueldoViewModel.marcarPago(id);

                if (resultado.IsSuccess)
                {
                    var sueldo = resultado.Value;
                    Console.WriteLine($"[ÉXITO] Sueldo marcado como pagado correctamente");
                    Console.WriteLine($"Monto pagado: ${sueldo.Monto_Pagado:F2}");
                    Console.WriteLine($"Fecha de pago: {sueldo.FechaDePago}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo marcar el sueldo como pagado: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al marcar sueldo como pagado: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba la eliminación de un sueldo
        /// </summary>
        public static async Task ProbarEliminarSueldo(int id)
        {
            Console.WriteLine($"\n=== ELIMINANDO SUELDO ID: {id} ===");

            try
            {
                var sueldoViewModel = new SueldoViewModel();
                var resultado = await sueldoViewModel.EliminarAsync(id);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Sueldo eliminado correctamente: {resultado.Value}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo eliminar el sueldo: {resultado.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Al eliminar sueldo: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba las validaciones del sueldo con varios casos erróneos
        /// </summary>
        public static async Task ProbarValidacionesSueldo()
        {
            Console.WriteLine("\n=== PROBANDO VALIDACIONES DE SUELDO ===");

            DateOnly fechaDesde = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
            DateOnly fechaHasta = DateOnly.FromDateTime(DateTime.Today);

            try
            {
                var sueldoViewModel = new SueldoViewModel();

                // Caso 1: Chofer inexistente
                Console.WriteLine("\n--- Caso 1: Chofer inexistente ---");
                var resultado1 = await sueldoViewModel.CrearAsync(
                    nombre_chofer: "ChoferQueNoExiste9999",
                    pagoDesde: fechaDesde,
                    pagoHasta: fechaHasta,
                    fechaPago: null,
                    patente_camion: null
                );

                if (!resultado1.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado1.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió chofer inexistente");

                // Caso 2: Fechas inválidas (desde > hasta)
                Console.WriteLine("\n--- Caso 2: Fechas inválidas (desde posterior a hasta) ---");
                var resultado2 = await sueldoViewModel.CrearAsync(
                    nombre_chofer: "Mili",
                    pagoDesde: fechaHasta,
                    pagoHasta: fechaDesde,
                    fechaPago: null,
                    patente_camion: null
                );

                if (!resultado2.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado2.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió fechas inválidas");

                // Caso 3: Camión inexistente
                Console.WriteLine("\n--- Caso 3: Camión inexistente ---");
                var resultado3 = await sueldoViewModel.CrearAsync(
                    nombre_chofer: "Mili",
                    pagoDesde: fechaDesde,
                    pagoHasta: fechaHasta,
                    fechaPago: null,
                    patente_camion: "NOEXISTE999"
                );

                if (!resultado3.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado3.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió camión inexistente");

                // Caso 4: Sin parámetros válidos
                Console.WriteLine("\n--- Caso 4: Sin filtros válidos ---");
                var resultado4 = await sueldoViewModel.ObtenerTodosAsync(null, null);

                if (!resultado4.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado4.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió consulta sin filtros");

                // Caso 5: ID inválido para marcar pago
                Console.WriteLine("\n--- Caso 5: ID inválido para marcar pago ---");
                var resultado5 = await sueldoViewModel.marcarPago(-1);

                if (!resultado5.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado5.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió ID inválido");

                // Caso 6: Marcar como pagado un sueldo ya pagado
                Console.WriteLine("\n--- Caso 6: Marcar como pagado un sueldo ya pagado ---");

                // Primero creamos un sueldo y lo marcamos como pagado
                var resultadoCreacion = await sueldoViewModel.CrearAsync(
                    nombre_chofer: "Mili",
                    pagoDesde: fechaDesde,
                    pagoHasta: fechaHasta,
                    fechaPago: DateOnly.FromDateTime(DateTime.Today),
                    patente_camion: "HIJ429"
                );

                if (resultadoCreacion.IsSuccess)
                {
                    // Intentamos marcarlo como pagado nuevamente
                    var resultado6 = await sueldoViewModel.marcarPago(resultadoCreacion.Value);

                    if (!resultado6.IsSuccess)
                        Console.WriteLine($"[ÉXITO] Validación correcta: {resultado6.Error}");
                    else
                        Console.WriteLine($"[ERROR] La validación falló: Permitió marcar como pagado un sueldo ya pagado");

                    // Limpiamos el sueldo de prueba
                    await sueldoViewModel.EliminarAsync(resultadoCreacion.Value);
                }
                else
                {
                    Console.WriteLine($"[ADVERTENCIA] No se pudo crear sueldo para la prueba: {resultadoCreacion.Error}");
                }

                Console.WriteLine("\nValidaciones de sueldo completadas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Error al probar validaciones: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Prueba el flujo completo de creación, modificación y eliminación de sueldo
        /// </summary>
        public static async Task ProbarFlujoCompletoSueldo()
        {
            Console.WriteLine("\n=== PROBANDO FLUJO COMPLETO DE SUELDO ===");

            try
            {
                var sueldoViewModel = new SueldoViewModel();

                // 1. Crear un sueldo
                Console.WriteLine("Paso 1: Creando sueldo...");
                var resultadoCreacion = await sueldoViewModel.CrearAsync(
                    nombre_chofer: "Mili",
                    pagoDesde: new DateOnly(2025, 4, 1),
                    pagoHasta: new DateOnly(2025, 4, 30),
                    fechaPago: null,
                    patente_camion: "HIJ429"
                );

                if (!resultadoCreacion.IsSuccess)
                {
                    Console.WriteLine($"[ERROR] No se pudo crear el sueldo: {resultadoCreacion.Error}");
                    return;
                }

                int idSueldo = resultadoCreacion.Value;
                Console.WriteLine($"[ÉXITO] Sueldo creado con ID: {idSueldo}");

                // 2. Marcar como pagado
                Console.WriteLine("Paso 2: Marcando como pagado...");
                var resultadoPago = await sueldoViewModel.marcarPago(idSueldo);

                if (resultadoPago.IsSuccess)
                    Console.WriteLine("[ÉXITO] Sueldo marcado como pagado");
                else
                    Console.WriteLine($"[ERROR] No se pudo marcar como pagado: {resultadoPago.Error}");

                // 3. Verificar en la lista
                Console.WriteLine("Paso 3: Verificando en la lista...");
                var resultadoLista = await sueldoViewModel.ObtenerTodosAsync("HIJ429", "Mili");

                if (resultadoLista.IsSuccess)
                {
                    var sueldoEncontrado = resultadoLista.Value.Find(s => s.idSueldo == idSueldo);
                    if (sueldoEncontrado != null)
                        Console.WriteLine($"[ÉXITO] Sueldo encontrado en la lista: ${sueldoEncontrado.Monto_Pagado:F2}");
                    else
                        Console.WriteLine("[ERROR] Sueldo no encontrado en la lista");
                }
                else
                {
                    Console.WriteLine($"[ERROR] No se pudo obtener la lista: {resultadoLista.Error}");
                }

                // 4. Limpiar (eliminar)
                Console.WriteLine("Paso 4: Limpiando datos de prueba...");
                var resultadoEliminacion = await sueldoViewModel.EliminarAsync(idSueldo);

                if (resultadoEliminacion.IsSuccess)
                    Console.WriteLine("[ÉXITO] Sueldo eliminado correctamente");
                else
                    Console.WriteLine($"[ERROR] No se pudo eliminar: {resultadoEliminacion.Error}");

                Console.WriteLine("\n[ÉXITO] Flujo completo ejecutado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Error en flujo completo: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Prueba casos específicos de eliminación de sueldos
        /// </summary>
        public static async Task ProbarEliminacionEspecifica()
        {
            Console.WriteLine("\n=== PROBANDO CASOS ESPECÍFICOS DE ELIMINACIÓN ===");

            try
            {
                var sueldoViewModel = new SueldoViewModel();

                // Caso 1: Eliminar sueldo inexistente
                Console.WriteLine("\n--- Caso 1: Eliminar sueldo inexistente ---");
                var resultado1 = await sueldoViewModel.EliminarAsync(99999);

                if (!resultado1.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado1.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió eliminar sueldo inexistente");

                // Caso 2: Eliminar con ID inválido
                Console.WriteLine("\n--- Caso 2: Eliminar con ID inválido ---");
                var resultado2 = await sueldoViewModel.EliminarAsync(-1);

                if (!resultado2.IsSuccess)
                    Console.WriteLine($"[ÉXITO] Validación correcta: {resultado2.Error}");
                else
                    Console.WriteLine($"[ERROR] La validación falló: Permitió ID inválido para eliminación");

                Console.WriteLine("\nCasos específicos de eliminación completados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPCIÓN] Error en pruebas de eliminación: {ex.Message}");
            }
        }
    }
}
