using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class CamionRepository
    {
        private readonly ApplicationDbContext _context;

        public CamionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                // Intentar comprobar si la conexión a la base de datos es exitosa
                bool puedeConectar = await _context.Database.CanConnectAsync();
                Console.WriteLine("rompió acá no??");
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
                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }


        public async Task<Camion> InsertarCamionAsync(float peso_max, float tara, string patente)
        {
            try
            {

                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var camion = new Camion(peso_max, tara, patente);

                // Agregar el camión a la base de datos (esto solo marca el objeto para insertar)
                _context.Camiones.Add(camion);

                Console.WriteLine($"SQL a ejecutar: {_context.ChangeTracker.DebugView.LongView}");

                // Guardar los cambios en la base de datos (aquí se genera el SQL real y se ejecuta)
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return camion;
                }
                Console.WriteLine("No se insertó ningún registro");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar camión: {ex.Message}");
                // Para debuggear, también es útil ver la excepción completa:
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<CamionDTO>> ObtenerCamionesAsync()
        {
            try
            {
                var camiones = await _context.Camiones.Select(c => new CamionDTO
                {
                    peso_max = c.peso_max,
                    tara = c.tara,
                    Patente = c.Patente
                }).ToListAsync();

                return camiones;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar camión: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }

        }

        public async Task<bool> Actualizar(int id, float? peso_max, float? tara, string? patente)
        {
            try
            {
                var camion = await _context.Camiones.FindAsync(id);
                if (camion == null)
                {
                    return false;
                }

                if (peso_max != null)
                {
                    camion.peso_max = (float)peso_max;
                }
                if (tara != null)
                {
                    camion.tara = (float)tara;
                }
                if (patente != null)
                {
                    camion.Patente = patente;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar camión: {ex.Message}");
                return false;
            }
        }

        internal async Task<CamionDTO> ObtenerPorId(int id)
        {
            Camion camion = await _context.Camiones.FindAsync(id);
            if (camion != null)
            {
                CamionDTO nuevo = new CamionDTO(camion.peso_max, camion.tara, camion.Patente);
                return nuevo;
            }
            return null;
        }

        public async Task<bool> EliminarCamionAsync(int id)
        {
            try
            {
                var camion = await _context.Camiones.FindAsync(id);

                if (camion == null)
                    return false;

                _context.Camiones.Remove(camion);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
    }
}