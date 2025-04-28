using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ViajeRepository
    {
        private readonly ApplicationDbContext _context;

        public ViajeRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                var puedeConectar = await _context.Database.CanConnectAsync();

                if (puedeConectar)
                    Console.WriteLine("Conexión exitosa a la base de datos.");
                else
                    Console.WriteLine(MensajeError.errorConexion());
                

                return puedeConectar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }

        // CREATE - Insertar un nuevo viaje
        public async Task<int> InsertarAsync(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            float kg,
            string carga,
            int cliente,
            int camion,
            float km,
            float tarifa,
            string nombreChofer
            )
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }

                var camionEntity = await _context.Camiones.FindAsync(camion);

                var viaje = new Viaje(fechaInicio, lugarPartida, destino, remito, kg,
                    carga, cliente, camion, km, tarifa, nombreChofer);

                _context.Viajes.Add(viaje);

                // Guardar los cambios en la base de datos
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados == 0)
                {
                    Console.WriteLine("No se insertó ningún registro");
                    return -1;
                }
                // Si se insertó correctamente, devolver el viaje como DTO

                var clienteEntity = await _context.Clientes.FindAsync(cliente);

                if (clienteEntity != null && camionEntity != null)
                    return viaje.Id;

                return -1;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine($"Error al insertar viaje: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                return -1;
            }
        }

        // READ - Obtener todos los viajes
        public async Task<List<ViajeDTO>> ObtenerTodosAsync()
        {
            try
            {
                var viajes = await _context.Viajes
                    .AsNoTracking() // Para mejorar performance en queries de solo lectura
                    .Include(v => v.ClienteNavigation)
                    .Select(v => new ViajeDTO
                    {
                        FechaInicio = v.FechaInicio,
                        LugarPartida = v.LugarPartida,
                        Destino = v.Destino,
                        Remito = v.Remito,
                        Kg = v.Kg,
                        Carga = v.Carga,
                        NombreCliente = v.ClienteNavigation.Nombre,
                        NombreChofer = v.NombreChofer,
                        Km = v.Km,
                        Tarifa = v.Tarifa
                    }).ToListAsync();

                if (viajes.Count == 0)
                    Console.WriteLine("No hay viajes");
                

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<ViajeDTO>();
            }
        }

        // READ - Obtener un viaje por ID
        public async Task<ViajeDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var viaje = await _context.Viajes
                    .AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (viaje == null)
                    return null;
                

                var viajeDTO = new ViajeDTO
                {
                    FechaInicio = viaje.FechaInicio,
                    LugarPartida = viaje.LugarPartida,
                    Destino = viaje.Destino,
                    Remito = viaje.Remito,
                    Kg = viaje.Kg,
                    Carga = viaje.Carga,
                    NombreCliente = viaje.ClienteNavigation.Nombre,
                    NombreChofer = viaje.NombreChofer,
                    Km = viaje.Km,
                    Tarifa = viaje.Tarifa,
                };

                return viajeDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viaje: {ex.Message}");
                return null;
            }
        }

        // READ - Obtener viajes con filtros
        public async Task<List<ViajeDTO>> ObtenerPorFechaYCamionAsync(int camionId,
                                                            DateOnly? fechaInicio = null,
                                                            DateOnly? fechaFin = null
                                                            )
        {
            try
            {
                // Comenzamos con una consulta que incluye todos los viajes
                var query = _context.Viajes.AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .Where(v => v.Camion == camionId);

                // Aplicamos los filtros según los parámetros proporcionados
                if (fechaInicio.HasValue)
                    query = query.Where(v => v.FechaInicio >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.FechaInicio <= fechaFin.Value);

                var viajes = await query.Select(v => new ViajeDTO
                {
                    FechaInicio = v.FechaInicio,
                    LugarPartida = v.LugarPartida,
                    Destino = v.Destino,
                    Remito = v.Remito,
                    Kg = v.Kg,
                    Carga = v.Carga,
                    NombreCliente = v.ClienteNavigation.Nombre,
                    NombreChofer = v.NombreChofer,
                    Km = v.Km,
                    Tarifa = v.Tarifa,
                }).ToListAsync();

                Console.WriteLine($"Se encontraron {viajes.Count} viajes.");

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al filtrar viajes: {ex.Message}");
                return new List<ViajeDTO>();
            }
        }

        // UPDATE - Actualizar sólo campos específicos de un viaje
        public async Task<bool> ActualizarAsync(
            int id,
            DateOnly? fechaInicio = null,
            string lugarPartida = null,
            string destino = null,
            int? remito = null,
            string carga = null,
            float? kg = null,
            int? cliente = null,
            int? camion = null,
            float? tarifa = null,
            float? km = null,
            string nombreChofer = null
            )
        {
            try
            {
                var viaje = await _context.Viajes.FindAsync(id);

                if (viaje == null)
                    return false;

                // Actualizar sólo los campos proporcionados
                if (fechaInicio.HasValue)
                    viaje.FechaInicio = fechaInicio.Value;

                if (!string.IsNullOrWhiteSpace(lugarPartida))
                    viaje.LugarPartida = lugarPartida;

                if (!string.IsNullOrWhiteSpace(destino))
                    viaje.Destino = destino;

                if (remito.HasValue)
                    viaje.Remito = remito.Value;
                
                if (!string.IsNullOrWhiteSpace(carga))
                    viaje.Carga = carga;

                if (kg.HasValue) 
                    viaje.Kg = kg.Value;

                if (cliente.HasValue)
                    viaje.Cliente = cliente.Value;

                if (camion.HasValue)
                    viaje.Camion = camion.Value;

                if (km.HasValue)
                    viaje.Km = km.Value;

                if (tarifa.HasValue)
                    viaje.Tarifa = tarifa.Value;

                if (!string.IsNullOrWhiteSpace(nombreChofer))
                    viaje.NombreChofer = nombreChofer;


                int registorsAfectados = await _context.SaveChangesAsync();

                if (registorsAfectados == 0)
                {
                    Console.WriteLine("No se actualizó ningún registro");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar campos del viaje: {ex.Message}");
                return false;
            }
        }

        // DELETE - Eliminar un viaje
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var viaje = await _context.Viajes.FindAsync(id);

                if (viaje == null)
                    return false;

                // Eliminar el viaje
                _context.Viajes.Remove(viaje);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar viaje: {ex.Message}");
                return false;
            }
        }

        // MÉTODOS ESPECÍFICOS

        // Obtener viajes por camión
        public async Task<List<ViajeDTO>> ObtenerPorCamionAsync(int camionId)
        {
            try
            {
                var viajes = await _context.Viajes
                    .AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .Where(v => v.Camion == camionId)
                    .Select(v => new ViajeDTO
                    {
                        FechaInicio = v.FechaInicio,
                        LugarPartida = v.LugarPartida,
                        Destino = v.Destino,
                        Remito = v.Remito,
                        Kg = v.Kg,
                        Carga = v.Carga,
                        NombreCliente = v.ClienteNavigation.Nombre,
                        NombreChofer = v.NombreChofer,
                        Km = v.Km,
                        Tarifa = v.Tarifa,
                    }).ToListAsync();

                Console.WriteLine($"Se encontraron {viajes.Count} viajes para el camión con ID {camionId}.");

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes con detalles: {ex.Message}");
                return new List<ViajeDTO>();
            }
        }

        // Obtener viajes por cliente
        public async Task<List<ViajeMixtoDTO>> ObtenerViajeMixtoPorClienteAsync(int clienteId)
        {
            try
            {

                var viajes = await _context.Viajes
                    .AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .Include(v => v.CamionNavigation)
                    .Where(v => v.Cliente == clienteId)
                    .Select(v => new ViajeMixtoDTO
                    {
                        Fecha_salida = v.FechaInicio,
                        Origen = v.LugarPartida,
                        Destino = v.Destino,
                        Remito = v.Remito,
                        Kg = v.Kg,
                        Carga = v.Carga,
                        Nombre_chofer = v.NombreChofer,
                        Km = v.Km,
                        Tarifa = v.Tarifa,
                        Camion = v.CamionNavigation.Patente
                    }).ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por cliente: {ex.Message}");
                return new List<ViajeMixtoDTO>();
            }
        }

        public async Task<List<ViajeDTO>> ObtenerPorClienteAsync(int clienteId)
        {
            try
            {

                var viajes = await _context.Viajes
                    .AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .Where(v => v.Cliente == clienteId)
                    .Select(v => new ViajeDTO
                    {
                        FechaInicio = v.FechaInicio,
                        LugarPartida = v.LugarPartida,
                        Destino = v.Destino,
                        Remito = v.Remito,
                        Kg = v.Kg,
                        Carga = v.Carga,
                        NombreCliente = v.ClienteNavigation.Nombre,
                        NombreChofer = v.NombreChofer,
                        Km = v.Km,
                        Tarifa = v.Tarifa,
                    }).ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por cliente: {ex.Message}");
                Console.WriteLine(ex.InnerException);
                return new List<ViajeDTO>();
            }
        }

        public async Task<List<ViajeDTO>> ObtenerPorChoferAsync(string chofer)
        {
            try
            {
                var viajes = await _context.Viajes
                    .AsNoTracking()
                    .Include(v => v.ClienteNavigation)
                    .Where(v => v.NombreChofer == chofer)
                    .Select(v => new ViajeDTO()
                    {
                        FechaInicio = v.FechaInicio,
                        LugarPartida = v.LugarPartida,
                        Destino = v.Destino,
                        Remito = v.Remito,
                        Kg = v.Kg,
                        Carga = v.Carga,
                        NombreCliente = v.ClienteNavigation.Nombre,
                        NombreChofer = v.NombreChofer,
                        Km = v.Km,
                        Tarifa = v.Tarifa,
                    }).ToListAsync();

                if (viajes.Count == 0)
                {
                    Console.WriteLine($"No hay viajes con ese chofer");
                }

                return viajes;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por chofer: {ex.Message}");
                return new List<ViajeDTO>();
            }
        }
    }
}