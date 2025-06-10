using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.DTOs;

namespace Proyecto_camiones.Tests
{
    public static class CamionTest
    {
        private static CamionViewModel _camionVM = new CamionViewModel();
        private static ViajeViewModel _viajeVM = new ViajeViewModel();

        public static async Task EjecutarTodasLasPruebasCamion()
        {
            Console.WriteLine("\n======= INICIANDO PRUEBAS DE CAMIÓN =======\n");

            try
            {
                await EjecutarPruebasBasicas();
                await EjecutarPruebasValidacion();
                await EjecutarPruebasEliminacionConPagos();

                Console.WriteLine("\n======= PRUEBAS COMPLETADAS =======\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR FATAL] Error en las pruebas: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        #region PRUEBAS BÁSICAS CRUD

        public static async Task EjecutarPruebasBasicas()
        {
            Console.WriteLine("=== PRUEBAS BÁSICAS CRUD ===\n");

            await ProbarCreacionExitosa();
            await ProbarObtenerPorId();
            await ProbarObtenerTodos();
            await ProbarActualizacionExitosa();
            await ProbarEliminacionBasica();

            Console.WriteLine("Pruebas básicas completadas\n");
        }

        public static async Task ProbarCreacionExitosa()
        {
            Console.WriteLine("--- PRUEBA: CREACIÓN EXITOSA ---");

            var resultado = await _camionVM.InsertarAsync("TEST001", "Juan Pérez");

            if (resultado.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Camión creado con ID: {resultado.Value}");
                await _camionVM.EliminarAsync(resultado.Value);
            }
            else
            {
                Console.WriteLine($"[ERROR] No se pudo crear: {resultado.Error}");
            }
        }

        public static async Task ProbarObtenerPorId()
        {
            Console.WriteLine("\n--- PRUEBA: OBTENER POR ID ---");

            var creacion = await _camionVM.InsertarAsync("TEST002", "María García");
            if (!creacion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión para la prueba");
                return;
            }

            int idCreado = creacion.Value;

            var resultado = await _camionVM.ObtenerPorId(idCreado);

            if (resultado.IsSuccess)
            {
                var camion = resultado.Value;
                Console.WriteLine($"[ÉXITO] Camión encontrado - ID: {camion.Id}, Patente: {camion.Patente}, Chofer: {camion.Nombre_Chofer}");
            }
            else
            {
                Console.WriteLine($"[ERROR] No se encontró: {resultado.Error}");
            }

            var inexistente = await _camionVM.ObtenerPorId(99999);
            if (!inexistente.IsSuccess)
            {
                Console.WriteLine("[ÉXITO] Manejo correcto de ID inexistente");
            }

            await _camionVM.EliminarAsync(idCreado);
        }

        public static async Task ProbarObtenerTodos()
        {
            Console.WriteLine("\n--- PRUEBA: OBTENER TODOS ---");

            var camionesCreados = new List<int>();

            for (int i = 1; i <= 3; i++)
            {
                var creacion = await _camionVM.InsertarAsync($"TEST00{i}", $"Chofer {i}");
                if (creacion.IsSuccess)
                {
                    camionesCreados.Add(creacion.Value);
                }
            }

            var resultado = await _camionVM.ObtenerTodosAsync();

            if (resultado.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Encontrados {resultado.Value.Count} camiones");
                int encontrados = resultado.Value.Count(c => camionesCreados.Contains(c.Id));
                Console.WriteLine($"Camiones de prueba encontrados: {encontrados}/{camionesCreados.Count}");
            }
            else
            {
                Console.WriteLine($"[ERROR] No se pudieron obtener: {resultado.Error}");
            }

            foreach (int id in camionesCreados)
            {
                await _camionVM.EliminarAsync(id);
            }
        }

        public static async Task ProbarActualizacionExitosa()
        {
            Console.WriteLine("\n--- PRUEBA: ACTUALIZACIÓN EXITOSA ---");

            var creacion = await _camionVM.InsertarAsync("TEST004", "Chofer Original");
            if (!creacion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión para la prueba");
                return;
            }

            int idCreado = creacion.Value;

            var actualPatente = await _camionVM.ActualizarAsync(idCreado, "TEST999", null);
            if (actualPatente.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Patente actualizada: {actualPatente.Value.Patente}");
            }

            var actualChofer = await _camionVM.ActualizarAsync(idCreado, null, "Chofer Actualizado");
            if (actualChofer.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Chofer actualizado: {actualChofer.Value.Nombre_Chofer}");
            }

            await _camionVM.EliminarAsync(idCreado);
        }

        public static async Task ProbarEliminacionBasica()
        {
            Console.WriteLine("\n--- PRUEBA: ELIMINACIÓN BÁSICA ---");

            var creacion = await _camionVM.InsertarAsync("TEST005", "Chofer Eliminar");
            if (!creacion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión para la prueba");
                return;
            }

            int idCreado = creacion.Value;

            var eliminacion = await _camionVM.EliminarAsync(idCreado);

            if (eliminacion.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Eliminación exitosa: {eliminacion.Value}");

                var verificacion = await _camionVM.ObtenerPorId(idCreado);
                if (!verificacion.IsSuccess)
                {
                    Console.WriteLine("[CONFIRMADO] El camión ya no existe en la base de datos");
                }
            }
            else
            {
                Console.WriteLine($"[ERROR] No se pudo eliminar: {eliminacion.Error}");
                await _camionVM.EliminarAsync(idCreado);
            }
        }

        #endregion

        #region PRUEBAS DE VALIDACIÓN

        public static async Task EjecutarPruebasValidacion()
        {
            Console.WriteLine("=== PRUEBAS DE VALIDACIÓN ===\n");

            await ProbarCreacionConDatosInvalidos();
            await ProbarActualizacionConDatosInvalidos();

            Console.WriteLine("Pruebas de validación completadas\n");
        }

        public static async Task ProbarCreacionConDatosInvalidos()
        {
            Console.WriteLine("--- PRUEBA: CREACIÓN CON DATOS INVÁLIDOS ---");

            var casosInvalidos = new[]
            {
                (patente: "", chofer: "Chofer Válido", descripcion: "Patente vacía"),
                (patente: "ABC123", chofer: "", descripcion: "Chofer vacío"),
                (patente: "", chofer: "", descripcion: "Ambos vacíos")
            };

            foreach (var (patente, chofer, descripcion) in casosInvalidos)
            {
                var resultado = await _camionVM.InsertarAsync(patente, chofer);

                if (!resultado.IsSuccess)
                {
                    Console.WriteLine($"[ÉXITO] {descripcion}: Rechazado correctamente - {resultado.Error}");
                }
                else
                {
                    Console.WriteLine($"[ERROR] {descripcion}: Aceptado cuando debería rechazarse - ID: {resultado.Value}");
                    await _camionVM.EliminarAsync(resultado.Value);
                }
            }
        }

        public static async Task ProbarActualizacionConDatosInvalidos()
        {
            Console.WriteLine("\n--- PRUEBA: ACTUALIZACIÓN CON DATOS INVÁLIDOS ---");

            var creacion = await _camionVM.InsertarAsync("VAL123", "Chofer Validación");
            if (!creacion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión para la prueba");
                return;
            }

            int idCreado = creacion.Value;

            var idInvalido = await _camionVM.ActualizarAsync(-1, "NUEVA", "Nuevo Chofer");
            if (!idInvalido.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] ID inválido rechazado: {idInvalido.Error}");
            }

            var sinCambios = await _camionVM.ActualizarAsync(idCreado, null, null);
            if (!sinCambios.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Actualización sin cambios rechazada: {sinCambios.Error}");
            }

            var idInexistente = await _camionVM.ActualizarAsync(99999, "INEXISTENTE", "Chofer Fantasma");
            if (!idInexistente.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] ID inexistente rechazado: {idInexistente.Error}");
            }

            await _camionVM.EliminarAsync(idCreado);
        }

        #endregion

        #region PRUEBAS DE ELIMINACIÓN CON CONTROL DE PAGOS

        public static async Task EjecutarPruebasEliminacionConPagos()
        {
            Console.WriteLine("=== PRUEBAS DE ELIMINACIÓN CON CONTROL DE PAGOS ===\n");

            await ProbarEliminacionCamionSinViajes();
            await ProbarEliminacionCamionConViajesSinPagos();
            await ProbarEliminacionCamionConPagosPendientes();

            Console.WriteLine("Pruebas de eliminación completadas\n");
        }

        public static async Task ProbarEliminacionCamionSinViajes()
        {
            Console.WriteLine("--- PRUEBA: ELIMINAR CAMIÓN SIN VIAJES ---");

            var creacion = await _camionVM.InsertarAsync("LIBRE001", "Chofer Sin Viajes");
            if (!creacion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión para la prueba");
                return;
            }

            int idCamion = creacion.Value;

            var eliminacion = await _camionVM.EliminarAsync(idCamion);

            if (eliminacion.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Camión sin viajes eliminado correctamente: {eliminacion.Value}");
            }
            else
            {
                Console.WriteLine($"[ERROR] No se pudo eliminar camión sin viajes: {eliminacion.Error}");
                await _camionVM.EliminarAsync(idCamion);
            }
        }

        public static async Task ProbarEliminacionCamionConViajesSinPagos()
        {
            Console.WriteLine("\n--- PRUEBA: ELIMINAR CAMIÓN CON VIAJES SIN PAGOS ---");

            int idCamion = -1;
            var viajesCreados = new List<int>();

            var creacionCamion = await _camionVM.InsertarAsync("VIAJES001", "Chofer Con Viajes");
            if (!creacionCamion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión");
                return;
            }

            idCamion = creacionCamion.Value;

            for (int i = 1; i <= 2; i++)
            {
                var viajeCreacion = await _viajeVM.CrearAsync(
                    fechaInicio: new DateOnly(2025, 4, 20 + i),
                    lugarPartida: "Origen Test",
                    destino: $"Destino {i}",
                    remito: 50000 + i,
                    carga: "Carga Test",
                    kg: 1000f,
                    cliente: "Cliente1",
                    camion: "VIAJES001",
                    km: 100f,
                    tarifa: 1000f,
                    nombreChofer: "Chofer Con Viajes",
                    porcentajeChofer: 0.18f
                );

                if (viajeCreacion.IsSuccess)
                {
                    viajesCreados.Add(viajeCreacion.Value);
                }
            }

            Console.WriteLine($"Se crearon {viajesCreados.Count} viajes para el camión");

            var eliminacion = await _camionVM.EliminarAsync(idCamion);

            if (eliminacion.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO] Camión con viajes eliminado: {eliminacion.Value}");
            }
            else
            {
                Console.WriteLine($"[CONTROLADO] Eliminación bloqueada: {eliminacion.Error}");

                if (eliminacion.Error.ToLower().Contains("pago") || eliminacion.Error.ToLower().Contains("pendiente"))
                {
                    Console.WriteLine("[ÉXITO] El sistema correctamente detectó pagos pendientes");
                }
            }

            foreach (int idViaje in viajesCreados)
            {
                await _viajeVM.EliminarAsync(idViaje);
            }

            if (idCamion > 0)
            {
                await _camionVM.EliminarAsync(idCamion);
            }
        }

        public static async Task ProbarEliminacionCamionConPagosPendientes()
        {
            Console.WriteLine("\n--- PRUEBA: BLOQUEO POR PAGOS PENDIENTES ---");

            int idCamion = -1;
            var viajesCreados = new List<int>();

            var creacionCamion = await _camionVM.InsertarAsync("PENDIENTE001", "Chofer Con Deudas");
            if (!creacionCamion.IsSuccess)
            {
                Console.WriteLine("[ERROR] No se pudo crear camión");
                return;
            }

            idCamion = creacionCamion.Value;

            var viajeCreacion = await _viajeVM.CrearAsync(
                fechaInicio: new DateOnly(2025, 4, 25),
                lugarPartida: "Origen Pendiente",
                destino: "Destino Pendiente",
                remito: 60001,
                carga: "Carga Pendiente",
                kg: 2000f,
                cliente: "Cliente A",
                camion: "PENDIENTE001",
                km: 200f,
                tarifa: 2000f,
                nombreChofer: "Chofer Con Deudas",
                porcentajeChofer: 0.18f
            );

            if (viajeCreacion.IsSuccess)
            {
                viajesCreados.Add(viajeCreacion.Value);
                Console.WriteLine($"Viaje con pagos pendientes creado: ID {viajeCreacion.Value}");
            }

            Console.WriteLine("Intentando eliminar camión con pagos pendientes...");
            var eliminacion = await _camionVM.EliminarAsync(idCamion);

            if (!eliminacion.IsSuccess)
            {
                Console.WriteLine($"[ÉXITO CRÍTICO] El sistema CORRECTAMENTE bloqueó la eliminación");
                Console.WriteLine($"Motivo: {eliminacion.Error}");

                if (eliminacion.Error.ToLower().Contains("pago") &&
                    eliminacion.Error.ToLower().Contains("pendiente"))
                {
                    Console.WriteLine("[VALIDACIÓN] El mensaje de error es específico sobre pagos pendientes");
                }
            }
            else
            {
                Console.WriteLine($"[FALLO CRÍTICO] El sistema PERMITIÓ eliminar camión con pagos pendientes");
                Console.WriteLine($"ESTO ES UN BUG GRAVE - Resultado: {eliminacion.Value}");
            }

            foreach (int idViaje in viajesCreados)
            {
                await _viajeVM.EliminarAsync(idViaje);
            }

            if (idCamion > 0)
            {
                await _camionVM.EliminarAsync(idCamion);
            }
        }

        #endregion

        #region MÉTODOS AUXILIARES

        public static async Task LimpiarDatosDePrueba()
        {
            Console.WriteLine("=== LIMPIEZA GENERAL DE DATOS DE PRUEBA ===");

            var camiones = await _camionVM.ObtenerTodosAsync();
            if (camiones.IsSuccess)
            {
                var camionesTest = camiones.Value.Where(c =>
                    c.Patente.StartsWith("TEST") ||
                    c.Patente.StartsWith("LIBRE") ||
                    c.Patente.StartsWith("VIAJES") ||
                    c.Patente.StartsWith("PENDIENTE"))
                    .ToList();

                Console.WriteLine($"Encontrados {camionesTest.Count} camiones de prueba para limpiar");

                foreach (var camion in camionesTest)
                {
                    var eliminacion = await _camionVM.EliminarAsync(camion.Id);
                    Console.WriteLine(eliminacion.IsSuccess
                        ? $"Eliminado: {camion.Patente}"
                        : $"Error eliminando {camion.Patente}: {eliminacion.Error}");
                }
            }
        }

        #endregion
    }
}
