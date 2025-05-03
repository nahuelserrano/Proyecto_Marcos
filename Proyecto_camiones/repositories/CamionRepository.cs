using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

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
            //MessageBox.Show("testeando conexión");
            try
            {
                // Intentar comprobar si la conexión a la base de datos es exitosa
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
                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
                MessageBox.Show($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }

        //agrego el signo de pregunta luego de Camion para decir que el result puede ser null
        public async Task<Camion?> InsertarAsync( string patente, string nombre)
        {
            try
            {

                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var camion = new Camion(patente, nombre);

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
                MessageBox.Show("Error de conexión" + ex.Message);
                MessageBox.Show("Error de conexión" + ex.InnerException);
                return null;
            }
        }

        public async Task<List<CamionDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var camiones = await _context.Camiones.Select(c => new CamionDTO
                {
                    Patente = c.Patente,
                    Nombre_Chofer = c.nombre_chofer
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

        public async Task<bool> ActualizarAsync(int id, string? patente, string? nombre)
        {
            try
            {
                var camion = await _context.Camiones.FindAsync(id);
                if (camion == null) return false;


                if (!string.IsNullOrEmpty(patente))  // Mejor verificación para strings
                {
                    camion.Patente = patente;
                }

                if (!string.IsNullOrEmpty(nombre))  // Mejor verificación para strings
                {
                    camion.nombre_chofer = nombre;
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

        internal async Task<CamionDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                Camion camion = await _context.Camiones.FindAsync(id);
                if (camion != null)
                {
                    CamionDTO nuevo = new CamionDTO(camion.Patente, camion.nombre_chofer);
                    return nuevo;
                }
                return null;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
            
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var camion = await _context.Camiones.FindAsync(id);
                Console.WriteLine("se encontró el camión");

                if (camion == null)
                    return false;

                _context.Camiones.Remove(camion);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return false;
            }
        }
    }
}