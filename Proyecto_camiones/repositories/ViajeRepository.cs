﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Viaje?> InsertarAsync(
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
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var viaje = new Viaje(fechaInicio, lugarPartida, destino, remito, kg, 
                    carga, cliente, camion, km, tarifa);

                // Agregar el viaje a la base de datos (esto solo marca el objeto para insertar)
                _context.Viajes.Add(viaje);

                // Guardar los cambios en la base de datos
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return viaje;
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
        public async Task<List<Viaje>> ObtenerTodosAsync()
        {
            try
            {
                var viajes = await _context.Viajes.ToListAsync();
                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Viaje>();
            }
        }

        // READ - Obtener un viaje por ID
        public async Task<Viaje> ObtenerPorIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID de viaje inválido");
                    return null;
                }

                var viaje = await _context.Viajes.FindAsync(id);
                return viaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viaje: {ex.Message}");
                return null;
            }
        }

        // READ - Obtener viajes con filtros
        public async Task<List<Viaje>> ObtenerPorFiltroAsync(DateOnly? fechaInicio = null,
                                                            DateOnly? fechaFin = null,
                                                            int? camionId = null)
        {
            try
            {
                // Comenzamos con una consulta que incluye todos los viajes
                IQueryable<Viaje> query = _context.Viajes;

                // Aplicamos los filtros según los parámetros proporcionados
                if (fechaInicio.HasValue)
                    query = query.Where(v => v.FechaInicio >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.FechaInicio <= fechaFin.Value);

                if (camionId.HasValue)
                    query = query.Where(v => v.Camion == camionId.Value);

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
                if (id <= 0)
                    return false;

                // Verificar si el viaje existe
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

                // Guardar los cambios
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
                if (id <= 0)
                    return false;

                // Buscar el viaje
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
        public async Task<List<Viaje>> ObtenerPorCamionAsync(int camionId)
        {
            try
            {
                if (camionId <= 0)
                    return new List<Viaje>();

                var viajes = await _context.Viajes
                    .Where(v => v.Camion == camionId)
                    .ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por camión: {ex.Message}");
                return new List<Viaje>();
            }
        }

        // Obtener viajes por cliente
        public async Task<List<Viaje>> ObtenerPorClienteAsync(int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                    return new List<Viaje>();

                var viajes = await _context.Viajes
                    .Where(v => v.Cliente == clienteId)
                    .ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por cliente: {ex.Message}");
                return new List<Viaje>();
            }
        }

        // Obtener viajes en un rango de fechas
        public async Task<List<Viaje>> ObtenerPorRangoFechasAsync(DateOnly fechaInicio, DateOnly fechaFin)
        {
            try
            {
                var viajes = await _context.Viajes
                    .Where(v => v.FechaInicio >= fechaInicio && v.FechaInicio <= fechaFin)
                    .ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener viajes por rango de fechas: {ex.Message}");
                return new List<Viaje>();
            }
        }

        // Obtener total de viajes por mes
        public async Task<Dictionary<string, int>> ObtenerTotalViajesPorMesAsync(int año)
        {
            try
            {
                var viajes = await _context.Viajes
                    .Where(v => v.FechaInicio.Year == año)
                    .ToListAsync();

                // Agrupar por mes y contar
                var viajesPorMes = viajes
                    .GroupBy(v => v.FechaInicio.Month)
                    .ToDictionary(
                        g => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                        g => g.Count()
                    );

                return viajesPorMes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de viajes por mes: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }
    }
}