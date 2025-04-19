using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;

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
                bool puedeConectar = await _context.Database.CanConnectAsync();

                if (puedeConectar)
                {
                    Console.WriteLine("Conexión exitosa a la base de datos.");
                }
                else
                {
                    Console.WriteLine("No se puede conectar a la base de datos.");
                }

                return puedeConectar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }

        // CREATE - Insertar un nuevo viaje
        public async Task<ViajeDTO?> InsertarAsync(
            DateOnly fechaInicio, 
            string lugarPartida,
            string destino, 
            int remito, 
            float kg,
            string carga, 
            int cliente, 
            int camion,
            float km,
            float tarifa
            )
        {
            try
            {
                Console.WriteLine(6);
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var viaje = new Viaje(fechaInicio, lugarPartida, destino, remito, kg, 
                    carga, cliente, camion, km, tarifa);

                _context.Viajes.Add(viaje);

                // Guardar los cambios en la base de datos
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    // Si se insertó correctamente, devolver el viaje como DTO
                   
                    _context.Viajes.LastOrDefaultAsync(v => v.Id == viaje.Id);
                }
                Console.WriteLine("No se insertó ningún registro");
                return null;
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
                return null;
            }
        }

        // READ - Obtener todos los viajes
        public async Task<List<ViajeDTO>> ObtenerTodosAsync()
        {
            try
            {
                var viajes = await _context.Viajes
                    .Join
                    (_context.Clientes,
                    viaje => viaje.Cliente,
                    cliente => cliente.Id,
                    (viaje, cliente) => new { Viaje = viaje, Cliente = cliente }
                    ).Join(
                        _context.Camiones,
                        combinado => combinado.Viaje.Camion,
                        camion => camion.Id,
                        (combinado, camion) => new ViajeDTO
                        {
                            FechaInicio = combinado.Viaje.FechaInicio,
                            LugarPartida = combinado.Viaje.LugarPartida,
                            Destino = combinado.Viaje.Destino,
                            Remito = combinado.Viaje.Remito,
                            Kg = combinado.Viaje.Kg,
                            Carga = combinado.Viaje.Carga,
                            NombreCliente = combinado.Cliente.Nombre,
                            NombreChofer = camion.nombre_chofer, // Directo del camión
                            Km = combinado.Viaje.Km,
                            Tarifa = combinado.Viaje.Tarifa
                        }
                    ).ToListAsync();
                
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
        public async Task<ViajeDTO> ObtenerPorIdAsync(int id)
        {
            try
            {
                var viajes = await _context.Viajes
                    .Join
                    (
                        _context.Clientes,
                        viaje => id,
                        cliente => cliente.Id,
                        (viaje, cliente) => new ViajeDTO
                        {
                            FechaInicio = viaje.FechaInicio,
                            LugarPartida = viaje.LugarPartida,
                            Destino = viaje.Destino,
                            Remito = viaje.Remito,
                            Kg = viaje.Kg,
                            Carga = viaje.Carga,
                            NombreCliente = cliente.Nombre,
                            NombreChofer = viaje.NombreChofer,
                            Km = viaje.Km,
                            Tarifa = viaje.Tarifa
                        }
                    ).FirstOrDefaultAsync();

                return viajes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // READ - Obtener viajes con filtros
        public async Task<List<Viaje>> ObtenerPorFechaYCamionAsync(int camionId,
                                                            DateOnly? fechaInicio = null,
                                                            DateOnly? fechaFin = null
                                                            )
        {
            try
            {
                // Comenzamos con una consulta que incluye todos los viajes
                IQueryable<Viaje> query = _context.Viajes;

                query
                    .Join(_context.Clientes,
                        viaje => viaje.Cliente,
                        cliente => cliente.Id,
                        (viaje, cliente) => new { Viaje = viaje, Cliente = cliente }
                        )
                    .Join(_context.Camiones,
                        combinado => combinado.Viaje.Camion,
                        camion => camionId,
                        (combinado, camion) => new ViajeDTO
                        {
                            FechaInicio = combinado.Viaje.FechaInicio,
                            LugarPartida = combinado.Viaje.LugarPartida,
                            Destino = combinado.Viaje.Destino,
                            Remito = combinado.Viaje.Remito,
                            Kg = combinado.Viaje.Kg,
                            Carga = combinado.Viaje.Carga,
                            NombreCliente = combinado.Cliente.Nombre,
                            NombreChofer = camion.nombre_chofer, // Directo del camión
                            Km = combinado.Viaje.Km,
                            Tarifa = combinado.Viaje.Tarifa,
                        }
                    );

                // Aplicamos los filtros según los parámetros proporcionados
                if (fechaInicio.HasValue)
                    query = query.Where(v => v.FechaInicio >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.FechaInicio <= fechaFin.Value);

                // Ejecutamos la consulta y devolvemos el resultado
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al filtrar viajes: {ex.Message}");
                return new List<Viaje>();
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
            float? km = null
            )
        {
            try
            {
                var viaje = await _context.Viajes.FindAsync(id);

                if (viaje == null)
                    return false;

                // Actualizar sólo los campos proporcionados
                if (!string.IsNullOrWhiteSpace(destino))
                    viaje.Destino = destino;

                if (!string.IsNullOrWhiteSpace(lugarPartida))
                    viaje.LugarPartida = lugarPartida;

                if (kg.HasValue)
                    viaje.Kg = kg.Value;

                if (remito.HasValue)
                    viaje.Remito = remito.Value;

                if (tarifa.HasValue)
                    viaje.Tarifa = tarifa.Value;

                if (cliente.HasValue)
                    viaje.Cliente = cliente.Value;

                if (camion.HasValue)
                    viaje.Camion = camion.Value;

                if (fechaInicio.HasValue)
                    viaje.FechaInicio = fechaInicio.Value;

                if (!string.IsNullOrWhiteSpace(carga))
                    viaje.Carga = carga;

                if (km.HasValue)
                    viaje.Km = km.Value;

                await _context.SaveChangesAsync();

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
                    .Join(
                        _context.Clientes,
                        viaje => viaje.Cliente,
                        cliente => cliente.Id,
                        (viaje, cliente) => new { Viaje = viaje, Cliente = cliente }
                    )
                    .Join(
                        _context.Camiones,
                        combinado => combinado.Viaje.Camion,
                        camion => camionId,
                        (combinado, camion) => new ViajeDTO
                        {
                            FechaInicio = combinado.Viaje.FechaInicio,
                            LugarPartida = combinado.Viaje.LugarPartida,
                            Destino = combinado.Viaje.Destino,
                            Remito = combinado.Viaje.Remito,
                            Kg = combinado.Viaje.Kg,
                            Carga = combinado.Viaje.Carga,
                            NombreCliente = combinado.Cliente.Nombre,
                            NombreChofer = camion.nombre_chofer, // Directo del camión
                            Km = combinado.Viaje.Km,
                            Tarifa = combinado.Viaje.Tarifa,
                        }
                    )
                    .ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes con detalles: {ex.Message}");
                return new List<ViajeDTO>();
            }
        }

        // Obtener viajes por cliente
        public async Task<List<ViajeDTO>> ObtenerPorClienteAsync(int clienteId)
        {
            try
            {

                var viajes = await _context.Viajes
                    .Join(
                        _context.Clientes,
                        viaje => viaje.Cliente,
                        cliente => clienteId,
                        (viaje, cliente) => new { Viaje = viaje, Cliente = cliente }
                    )
                    .Join(
                        _context.Camiones,
                        combinado => combinado.Viaje.Camion,
                        camion => camion.Id,
                        (combinado, camion) => new ViajeDTO
                        {
                            FechaInicio = combinado.Viaje.FechaInicio,
                            LugarPartida = combinado.Viaje.LugarPartida,
                            Destino = combinado.Viaje.Destino,
                            Remito = combinado.Viaje.Remito,
                            Kg = combinado.Viaje.Kg,
                            Carga = combinado.Viaje.Carga,
                            NombreCliente = combinado.Cliente.Nombre,
                            NombreChofer = camion.nombre_chofer, // Directo del camión
                            Km = combinado.Viaje.Km,
                            Tarifa = combinado.Viaje.Tarifa,
                        }
                    ).ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por cliente: {ex.Message}");
                return new List<ViajeDTO>();
            }
        }
    }
}