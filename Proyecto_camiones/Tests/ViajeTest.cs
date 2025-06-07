using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Tests
{
    public static class ViajeTest
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
                await ProbarObtenerViajesPorCamion("HIJ429");
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
                    Console.WriteLine($"  Precio total: ${viaje.Total:F2}");
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
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.Total:F2})");
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

        public static async Task ProbarObtenerViajesPorCamion(string patente)
        {
            Console.WriteLine($"\n=== OBTENIENDO VIAJES POR CAMIÓN ID: {patente} ===");
            ViajeViewModel vvm = new ViajeViewModel();

            try
            {
                var resultado = await vvm.ObtenerPorCamionAsync(patente);
                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados para el camión {patente}: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.Total:F2})");
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
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.Total:F2})");
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
                        Console.WriteLine($"  - {viaje.FechaInicio}: {viaje.LugarPartida} → {viaje.Destino} (${viaje.Total:F2})");
                        //Console.WriteLine(viaje.ToString());
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

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] Viajes encontrados: {resultado.Value.Count}");
                    foreach (var viaje in resultado.Value)
                    {
                        Console.WriteLine($"  - {viaje.Fecha_salida}: {viaje.Origen} → {viaje.Destino} (${viaje.Total:F2})");
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
            string? cliente = null,
            string? camion = null,
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

        /// <summary>
        /// Ejecuta todas las pruebas de validación en ViajeViewModel
        /// </summary>
        public static async Task ProbarValidacionesCompletas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS COMPLETAS DE VALIDACIÓN VIAJE =======\n");

            await ProbarCreacionExitosa();
            await ProbarRechazarFechaFutura();
            await ProbarRechazarKgNegativo();
            await ProbarRechazarTarifaNegativa();
            await ProbarRechazarPorcentajeInvalido();
            await ProbarRechazarClienteInexistente();
            await ProbarRechazarCamionInexistente();
            await ProbarRechazarRemitoDuplicado();
            await ProbarRechazarOrigenDestinoIguales();
            await ProbarRechazarTodosMalosExceptoClienteYCamion();
            await ProbarRechazarClienteYCamionMalos();

            Console.WriteLine("\n======= FINALIZADAS TODAS LAS PRUEBAS DE VALIDACIÓN VIAJE =======\n");
        }

        /// <summary>
        /// Ejecuta todas las pruebas que deberían fallar (casos inválidos)
        /// </summary>
        public static async Task ProbarValidacionesErroneas()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS DE VALIDACIONES ERRÓNEAS =======\n");

            await ProbarRechazarFechaFutura();
            await ProbarRechazarKgNegativo();
            await ProbarRechazarTarifaNegativa();
            await ProbarRechazarPorcentajeInvalido();
            await ProbarRechazarClienteInexistente();
            await ProbarRechazarCamionInexistente();
            await ProbarRechazarRemitoDuplicado();
            await ProbarRechazarOrigenDestinoIguales();
            await ProbarRechazarTodosMalosExceptoClienteYCamion();
            await ProbarRechazarClienteYCamionMalos();

            Console.WriteLine("\n======= FINALIZADAS PRUEBAS DE VALIDACIONES ERRÓNEAS =======\n");
        }

        /// <summary>
        /// Prueba básica de creación exitosa de viaje
        /// </summary>
        public static async Task ProbarCreacionExitosa()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: CREACIÓN EXITOSA DE VIAJE =====");
            var resultadoCreacionExitosa = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Buenos Aires",
                remito: 12345,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1", // Asegúrate que exista en la BD
                camion: "HIJ429",    // Asegúrate que exista en la BD
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (resultadoCreacionExitosa.IsSuccess)
            {
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Viaje creado con ID: {resultadoCreacionExitosa.Value}");

                // Limpieza: eliminar el viaje creado en la prueba
                Console.WriteLine($"Limpiando: Eliminando viaje de prueba ID {resultadoCreacionExitosa.Value}...");
                var eliminacion = await viajeVM.EliminarAsync(resultadoCreacionExitosa.Value);
                Console.WriteLine(eliminacion.IsSuccess ? "Eliminado con éxito" : $"Error al eliminar: {eliminacion.Error}");
            }
            else
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Error: {resultadoCreacionExitosa.Error}");
            }
        }

        /// <summary>
        /// Prueba que verifica el rechazo de fechas futuras
        /// </summary>
        public static async Task ProbarRechazarFechaFutura()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaFutura = new DateOnly(2026, 12, 31);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE FECHA FUTURA =====");
            var resultadoFechaFutura = await viajeVM.CrearAsync(
                fechaInicio: fechaFutura,
                lugarPartida: "Tandil",
                destino: "La Plata",
                remito: 12346,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoFechaFutura.IsSuccess && resultadoFechaFutura.Error.ToLower().Contains("fecha"))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó fecha futura como esperado: {resultadoFechaFutura.Error}");
            else if (resultadoFechaFutura.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó fecha futura cuando debería rechazarla. ID: {resultadoFechaFutura.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoFechaFutura.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con fecha: {resultadoFechaFutura.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de kg negativos
        /// </summary>
        public static async Task ProbarRechazarKgNegativo()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE KG NEGATIVO =====");
            var resultadoKgNegativo = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Azul",
                remito: 12347,
                carga: "Cereales",
                kg: -500.0f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoKgNegativo.IsSuccess && (resultadoKgNegativo.Error.ToLower().Contains("kg")))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó kg negativo como esperado: {resultadoKgNegativo.Error}");
            else if (resultadoKgNegativo.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó kg negativo cuando debería rechazarlo. ID: {resultadoKgNegativo.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoKgNegativo.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con kg: {resultadoKgNegativo.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de tarifas negativas
        /// </summary>
        public static async Task ProbarRechazarTarifaNegativa()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE TARIFA NEGATIVA =====");
            var resultadoTarifaNegativa = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Mar del Plata",
                remito: 12348,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 350.5f,
                tarifa: -100.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoTarifaNegativa.IsSuccess && resultadoTarifaNegativa.Error.ToLower().Contains("tarifa"))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó tarifa negativa como esperado: {resultadoTarifaNegativa.Error}");
            else if (resultadoTarifaNegativa.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó tarifa negativa cuando debería rechazarla. ID: {resultadoTarifaNegativa.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoTarifaNegativa.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con tarifa: {resultadoTarifaNegativa.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de porcentajes de chofer inválidos
        /// </summary>
        public static async Task ProbarRechazarPorcentajeInvalido()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE PORCENTAJE CHOFER INVÁLIDO =====");
            var resultadoPorcentajeInvalido = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Olavarría",
                remito: 12349,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 1.5f  // Mayor a 1 (100%)
            );

            if (!resultadoPorcentajeInvalido.IsSuccess && resultadoPorcentajeInvalido.Error.ToLower().Contains("porcentaje"))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó porcentaje inválido como esperado: {resultadoPorcentajeInvalido.Error}");
            else if (resultadoPorcentajeInvalido.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó porcentaje > 1 cuando debería rechazarlo. ID: {resultadoPorcentajeInvalido.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoPorcentajeInvalido.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con porcentaje: {resultadoPorcentajeInvalido.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de clientes inexistentes
        /// </summary>
        public static async Task ProbarRechazarClienteInexistente()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE CLIENTE INEXISTENTE =====");
            var resultadoClienteInexistente = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Necochea",
                remito: 12350,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "ClienteQueNoExiste12345", // Cliente que (espero) no exista
                camion: "HIJ429",
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoClienteInexistente.IsSuccess && resultadoClienteInexistente.Error.ToLower().Contains("cliente"))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó cliente inexistente como esperado: {resultadoClienteInexistente.Error}");
            else if (resultadoClienteInexistente.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó cliente inexistente cuando debería rechazarlo. ID: {resultadoClienteInexistente.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoClienteInexistente.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con cliente: {resultadoClienteInexistente.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de camiones inexistentes
        /// </summary>
        public static async Task ProbarRechazarCamionInexistente()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE CAMIÓN INEXISTENTE =====");
            var resultadoCamionInexistente = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Balcarce",
                remito: 12351,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "NOEXISTE123", // Camión que (espero) no exista
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoCamionInexistente.IsSuccess && resultadoCamionInexistente.Error.ToLower().Contains("camión"))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó camión inexistente como esperado: {resultadoCamionInexistente.Error}");
            else if (resultadoCamionInexistente.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó camión inexistente cuando debería rechazarlo. ID: {resultadoCamionInexistente.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoCamionInexistente.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con camión: {resultadoCamionInexistente.Error}");
        }

        /// <summary>
        /// Prueba que verifica el rechazo de remitos duplicados
        /// </summary>
        public static async Task ProbarRechazarRemitoDuplicado()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);
            int remitoUnico = new Random().Next(100000, 999999); // Generamos un número aleatorio grande
            int idViajeCreado = -1;

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE REMITO DUPLICADO =====");

            // Primer viaje con ese remito
            var resultadoRemito1 = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "La Plata",
                remito: remitoUnico,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            // Verificamos creación exitosa del primer viaje con ese remito
            if (!resultadoRemito1.IsSuccess)
            {
                Console.WriteLine($"[!] PREPARACIÓN PRUEBA FALLIDA - No se pudo crear el primer viaje: {resultadoRemito1.Error}");
                return;
            }

            idViajeCreado = resultadoRemito1.Value;
            Console.WriteLine($"[+] Primer viaje con remito {remitoUnico} creado con ID: {idViajeCreado}");

            try
            {
                // Intentamos crear un segundo viaje con el mismo remito
                var resultadoRemito2 = await viajeVM.CrearAsync(
                    fechaInicio: fechaValida,
                    lugarPartida: "Tandil",
                    destino: "Rosario",
                    remito: remitoUnico, // Mismo remito
                    carga: "Maquinaria",
                    kg: 800.0f,
                    cliente: "Cliente1",
                    camion: "HIJ429",
                    km: 700.0f,
                    tarifa: 6000.0f,
                    nombreChofer: "Chofer Test",
                    porcentajeChofer: 0.18f
                );

                if (!resultadoRemito2.IsSuccess && resultadoRemito2.Error.ToLower().Contains("remito"))
                    Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó remito duplicado como esperado: {resultadoRemito2.Error}");
                else if (resultadoRemito2.IsSuccess)
                {
                    Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó remito duplicado cuando debería rechazarlo. ID: {resultadoRemito2.Value}");

                    // Si se creó el segundo viaje (no debería), lo eliminamos
                    await LimpiarViajeCreado(viajeVM, resultadoRemito2.Value);
                }
                else
                    Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con remito: {resultadoRemito2.Error}");
            }
            finally
            {
                // Limpiar el primer viaje (siempre)
                if (idViajeCreado > 0)
                {
                    await LimpiarViajeCreado(viajeVM, idViajeCreado);
                }
            }
        }

        /// <summary>
        /// Prueba que verifica el rechazo de viajes con origen y destino iguales
        /// </summary>
        public static async Task ProbarRechazarOrigenDestinoIguales()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE ORIGEN Y DESTINO IGUALES =====");
            var resultadoOrigenDestino = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Tandil", // Mismo que origen
                remito: 12352,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "Cliente1",
                camion: "HIJ429",
                km: 0.0f, // Km debería ser > 0
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultadoOrigenDestino.IsSuccess &&
                (resultadoOrigenDestino.Error.ToLower().Contains("origen") ||
                 resultadoOrigenDestino.Error.ToLower().Contains("destino") ||
                 resultadoOrigenDestino.Error.ToLower().Contains("partida")))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó origen/destino iguales como esperado: {resultadoOrigenDestino.Error}");
            else if (resultadoOrigenDestino.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó origen/destino iguales cuando debería rechazarlo. ID: {resultadoOrigenDestino.Value}");
                await LimpiarViajeCreado(viajeVM, resultadoOrigenDestino.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con origen/destino: {resultadoOrigenDestino.Error}");
        }

        /// <summary>
        /// Prueba donde todos los datos son erróneos excepto cliente y camión
        /// </summary>
        public static async Task ProbarRechazarTodosMalosExceptoClienteYCamion()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaFutura = new DateOnly(2026, 12, 31);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE MÚLTIPLES CAMPOS ERRÓNEOS =====");
            var resultado = await viajeVM.CrearAsync(
                fechaInicio: fechaFutura, // Fecha futura (mal)
                lugarPartida: "", // Vacío (mal)
                destino: "", // Vacío (mal)
                remito: -1, // Negativo (mal)
                carga: "", // Vacío (mal)
                kg: -10.0f, // Negativo (mal)
                cliente: "Cliente1", // Existente (bien)
                camion: "HIJ429", // Existente (bien)
                km: -5.0f, // Negativo (mal)
                tarifa: -50.0f, // Negativa (mal)
                nombreChofer: "", // Vacío (mal)
                porcentajeChofer: 2.5f // Mayor a 1 (mal)
            );

            if (!resultado.IsSuccess)
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó múltiples campos erróneos como esperado: {resultado.Error}");
            else
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó múltiples campos erróneos cuando debería rechazarlos. ID: {resultado.Value}");
                await LimpiarViajeCreado(viajeVM, resultado.Value);
            }
        }

        /// <summary>
        /// Prueba donde sólo cliente y camión son erróneos (no existen)
        /// </summary>
        public static async Task ProbarRechazarClienteYCamionMalos()
        {
            ViajeViewModel viajeVM = new ViajeViewModel();
            DateOnly fechaValida = new DateOnly(2025, 4, 28);

            Console.WriteLine("\n===== PRUEBA: RECHAZO DE CLIENTE Y CAMIÓN INEXISTENTES =====");
            var resultado = await viajeVM.CrearAsync(
                fechaInicio: fechaValida,
                lugarPartida: "Tandil",
                destino: "Buenos Aires",
                remito: 12353,
                carga: "Cereales",
                kg: 1000.5f,
                cliente: "ClienteInexistente9876", // No existe (mal)
                camion: "PatenteFalsa9876", // No existe (mal)
                km: 350.5f,
                tarifa: 5000.0f,
                nombreChofer: "Chofer Test",
                porcentajeChofer: 0.18f
            );

            if (!resultado.IsSuccess &&
                (resultado.Error.ToLower().Contains("cliente") ||
                 resultado.Error.ToLower().Contains("camion")))
                Console.WriteLine($"[✓] PRUEBA EXITOSA - Rechazó cliente/camión inexistentes como esperado: {resultado.Error}");
            else if (resultado.IsSuccess)
            {
                Console.WriteLine($"[✗] PRUEBA FALLIDA - Aceptó cliente/camión inexistentes cuando debería rechazarlos. ID: {resultado.Value}");
                await LimpiarViajeCreado(viajeVM, resultado.Value);
            }
            else
                Console.WriteLine($"[?] PRUEBA INDETERMINADA - Error devuelto no relacionado con cliente/camión: {resultado.Error}");
        }

        /// <summary>
        /// Método auxiliar para limpiar viajes creados durante las pruebas
        /// </summary>
        private static async Task LimpiarViajeCreado(ViajeViewModel viajeVM, int id)
        {
            if (id <= 0) return;

            Console.WriteLine($"Limpiando: Eliminando viaje de prueba ID {id}...");
            var eliminacion = await viajeVM.EliminarAsync(id);
            Console.WriteLine(eliminacion.IsSuccess ? "Eliminado con éxito" : $"Error al eliminar: {eliminacion.Error}");
        }

        // Agregar estos métodos a la clase ViajeTest existente

        #region Pruebas Fuzzy Matching - Métodos simples para ViajeTest

        /// <summary>
        /// Ejecuta pruebas básicas de fuzzy matching para choferes
        /// </summary>
        public static async Task ProbarFuzzyMatchingBasico()
        {
            Console.WriteLine("\n======= PRUEBAS BÁSICAS DE FUZZY MATCHING =======\n");

            await ProbarActualizarViajeConTypoChofer();
            await ProbarCrearViajeConChoferSimilar();
            await ProbarVariosErroresEnNombreChofer();

            Console.WriteLine("\n======= FIN PRUEBAS FUZZY MATCHING =======\n");
        }

        /// <summary>
        /// Prueba actualizar un viaje con un typo en el nombre del chofer
        /// </summary>
        public static async Task ProbarActualizarViajeConTypoChofer()
        {
            Console.WriteLine("\n=== PRUEBA: ACTUALIZAR VIAJE CON TYPO EN CHOFER ===");
            ViajeViewModel vvm = new ViajeViewModel();
            DateOnly fecha = new DateOnly(2025, 4, 28);

            try
            {
                // Crear un viaje inicial
                var resultadoCreacion = await vvm.CrearAsync(
                    fechaInicio: fecha,
                    lugarPartida: "Tandil",
                    destino: "Buenos Aires",
                    remito: 99991,
                    carga: "Test Fuzzy",
                    kg: 1000f,
                    cliente: "Cliente1",
                    camion: "HIJ429",
                    km: 350f,
                    tarifa: 5000f,
                    nombreChofer: "Juan Pérez",
                    porcentajeChofer: 0.18f
                );

                if (!resultadoCreacion.IsSuccess)
                {
                    Console.WriteLine($"[✗] Error creando viaje: {resultadoCreacion.Error}");
                    return;
                }

                int idViaje = resultadoCreacion.Value;
                Console.WriteLine($"[✓] Viaje creado con ID: {idViaje}, chofer: Juan Pérez");

                // Actualizar con typo en el chofer (debería encontrar "Juan Pérez" usando fuzzy)
                // NOTA: Esto depende de que hayas implementado el fuzzy matching en ViajeService
                var resultadoActualizacion = await vvm.ActualizarAsync(
                    id: idViaje,
                    nombreChofer: "Juab Pérez" // Typo: 'b' en lugar de 'n'
                );

                if (resultadoActualizacion.IsSuccess)
                {
                    Console.WriteLine($"[✓] FUZZY EXITOSO: 'Juab Pérez' fue corregido automáticamente");

                    // Verificar el viaje actualizado
                    var viajeVerificacion = await vvm.ObtenerPorIdAsync(idViaje);
                    if (viajeVerificacion.IsSuccess)
                    {
                        Console.WriteLine($"[✓] Chofer final en BD: '{viajeVerificacion.Value.NombreChofer}'");
                    }
                }
                else
                {
                    Console.WriteLine($"[✗] FUZZY FALLÓ: {resultadoActualizacion.Error}");
                }

                // Limpiar
                await vvm.EliminarAsync(idViaje);
                Console.WriteLine($"[🗑] Viaje de prueba eliminado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] EXCEPCIÓN: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba crear un viaje con un chofer que tiene nombre similar a uno existente
        /// </summary>
        public static async Task ProbarCrearViajeConChoferSimilar()
        {
            Console.WriteLine("\n=== PRUEBA: CREAR VIAJE CON CHOFER SIMILAR ===");
            ViajeViewModel vvm = new ViajeViewModel();
            DateOnly fecha = new DateOnly(2025, 4, 28);

            try
            {
                // Crear viaje con chofer que ya existe pero con error tipográfico
                var resultado = await vvm.CrearAsync(
                    fechaInicio: fecha,
                    lugarPartida: "Tandil",
                    destino: "La Plata",
                    remito: 99992,
                    carga: "Test Fuzzy 2",
                    kg: 800f,
                    cliente: "Cliente1",
                    camion: "HIJ429",
                    km: 200f,
                    tarifa: 3000f,
                    nombreChofer: "Pepito",  // Existe "Pepito" en la BD
                    porcentajeChofer: 0.18f
                );

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"[✓] Viaje creado con chofer exacto: Pepito");
                    await vvm.EliminarAsync(resultado.Value);
                }

                // Ahora con un typo
                var resultado2 = await vvm.CrearAsync(
                    fechaInicio: fecha,
                    lugarPartida: "Tandil",
                    destino: "Necochea",
                    remito: 99993,
                    carga: "Test Fuzzy 3",
                    kg: 800f,
                    cliente: "Cliente1",
                    camion: "HIJ429",
                    km: 200f,
                    tarifa: 3000f,
                    nombreChofer: "Pepoto",  // Typo: 'o' en lugar de 'i'
                    porcentajeChofer: 0.18f
                );

                if (resultado2.IsSuccess)
                {
                    Console.WriteLine($"[✓] FUZZY EN CREACIÓN: 'Pepoto' debería usar chofer existente");

                    var viajeCreado = await vvm.ObtenerPorIdAsync(resultado2.Value);
                    if (viajeCreado.IsSuccess)
                    {
                        Console.WriteLine($"[✓] Chofer asignado: '{viajeCreado.Value.NombreChofer}'");
                    }

                    await vvm.EliminarAsync(resultado2.Value);
                }
                else
                {
                    Console.WriteLine($"[✗] Error con fuzzy en creación: {resultado2.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] EXCEPCIÓN: {ex.Message}");
            }
        }

        /// <summary>
        /// Prueba varios tipos de errores en nombres de choferes
        /// </summary>
        public static async Task ProbarVariosErroresEnNombreChofer()
        {
            Console.WriteLine("\n=== PRUEBA: VARIOS TIPOS DE ERRORES ===");
            ViajeViewModel vvm = new ViajeViewModel();
            DateOnly fecha = new DateOnly(2025, 4, 28);

            var casosTest = new List<(string choferConError, string descripcion)>
    {
        ("Pepit", "Eliminación de carácter"),
        ("Peppito", "Inserción de carácter"),
        ("Pepato", "Sustitución de carácter"),
        ("pepito", "Cambio de mayúsculas"),
        ("PEPITO", "Todo mayúsculas")
    };

            int contador = 99994;

            foreach (var (choferError, descripcion) in casosTest)
            {
                try
                {
                    Console.WriteLine($"\n--- Probando: {descripcion} ('{choferError}') ---");

                    var resultado = await vvm.CrearAsync(
                        fechaInicio: fecha,
                        lugarPartida: "Tandil",
                        destino: "Olavarría",
                        remito: contador++,
                        carga: $"Test {descripcion}",
                        kg: 500f,
                        cliente: "Cliente1",
                        camion: "HIJ429",
                        km: 100f,
                        tarifa: 2000f,
                        nombreChofer: choferError,
                        porcentajeChofer: 0.18f
                    );

                    if (resultado.IsSuccess)
                    {
                        var viajeCreado = await vvm.ObtenerPorIdAsync(resultado.Value);
                        if (viajeCreado.IsSuccess)
                        {
                            Console.WriteLine($"[✓] {descripcion}: '{choferError}' → '{viajeCreado.Value.NombreChofer}'");
                        }

                        await vvm.EliminarAsync(resultado.Value);
                    }
                    else
                    {
                        Console.WriteLine($"[✗] {descripcion} falló: {resultado.Error}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[✗] EXCEPCIÓN en {descripcion}: {ex.Message}");
                }
            }
        }

        #endregion

        #region Métodos de prueba directa del ChoferRepository

        /// <summary>
        /// Prueba directa de los métodos fuzzy del ChoferRepository
        /// </summary>
        public static async Task ProbarMetodosFuzzyRepository()
        {
            Console.WriteLine("\n======= PRUEBAS DIRECTAS DE CHOFER REPOSITORY =======\n");

            try
            {
                var choferRepo = new ChoferRepository();

                // Casos de prueba para búsqueda fuzzy
                var busquedasTest = new List<(string busqueda, string descripcion)>
                {
                    ("Juab", "Typo simple: b por n"),
                    ("Pepoto", "Typo simple: o por i"),
                    ("carlos", "Minúsculas"),
                    ("PEPITO", "Mayúsculas"),
                    ("Pepit", "Carácter faltante"),
                    ("Marria", "Carácter duplicado"),
                    ("Jośe", "Carácter especial")
                };

                Console.WriteLine("=== BÚSQUEDA CON LIKE ===");
                foreach (var (busqueda, descripcion) in busquedasTest)
                {
                    try
                    {
                        var resultadosLike = await choferRepo.BuscarChoferConLikeAsync(busqueda);
                        Console.WriteLine($"[LIKE] {descripcion}: '{busqueda}' → {resultadosLike.Count} resultados");

                        foreach (var chofer in resultadosLike.Take(3)) // Solo primeros 3
                        {
                            Console.WriteLine($"    - {chofer.Nombre}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[✗] Error LIKE '{busqueda}': {ex.Message}");
                    }
                }

                Console.WriteLine("\n=== BÚSQUEDA HÍBRIDA ===");
                foreach (var (busqueda, descripcion) in busquedasTest)
                {
                    try
                    {
                        var resultadoHibrido = await choferRepo.ObtenerPorSimilitudAsync(busqueda, 70.0);

                        if (resultadoHibrido.HasValue)
                        {
                            Console.WriteLine($"[✓] {descripcion}: '{busqueda}' → '{resultadoHibrido.Value.chofer.Nombre}' ({resultadoHibrido.Value.similitud:F1}%)");
                        }
                        else
                        {
                            Console.WriteLine($"[✗] {descripcion}: '{busqueda}' → Sin coincidencias");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[✗] Error HÍBRIDO '{busqueda}': {ex.Message}");
                    }
                }

                Console.WriteLine("\n=== PRUEBA DE RENDIMIENTO ===");
                var inicio = DateTime.Now;
                int busquedasRealizadas = 0;
                int busquedasExitosas = 0;

                foreach (var (busqueda, _) in busquedasTest)
                {
                    try
                    {
                        var resultado = await choferRepo.ObtenerPorSimilitudAsync(busqueda, 70.0);
                        busquedasRealizadas++;

                        if (resultado.HasValue)
                            busquedasExitosas++;
                    }
                    catch
                    {
                        busquedasRealizadas++;
                    }
                }

                var tiempoTotal = DateTime.Now - inicio;
                Console.WriteLine($"📊 Búsquedas: {busquedasRealizadas}, Exitosas: {busquedasExitosas}");
                Console.WriteLine($"📊 Tiempo total: {tiempoTotal.TotalMilliseconds:F2} ms");
                Console.WriteLine($"📊 Promedio: {tiempoTotal.TotalMilliseconds / busquedasRealizadas:F2} ms/búsqueda");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] ERROR GENERAL: {ex.Message}");
            }
        }

        #endregion


    }
}
