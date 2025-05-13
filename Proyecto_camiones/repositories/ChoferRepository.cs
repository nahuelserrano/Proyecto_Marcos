using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ChoferRepository
    {
        private readonly ApplicationDbContext _context;

        public ChoferRepository()
        {
            this._context = General.obtenerInstancia();
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

        // CRUD OPERATIONS

        // CREATE - Insertar un nuevo chofer
        public async Task<int> InsertarAsync(string nombre)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1; // Mejor retornar un valor específico de error que null
                }

                var chofer = new Chofer(nombre);

                _context.Choferes.Add(chofer);

                int registrosafectados = await _context.SaveChangesAsync();

                if (registrosafectados > 0)
                    return chofer.Id;
                    
                return -1;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar chofer: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1; // Mejor retornar un valor específico de error
            }
        }

        // READ - Obtener todos los choferes
        public async Task<List<Chofer>> ObtenerTodosAsync()
        {
            try
            {
                var choferes = await _context.Choferes.ToListAsync();

                return choferes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener choferes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Chofer>();
            }
        }

        // READ - Obtener un chofer específico por ID
        public async Task<Chofer?> ObtenerPorIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID inválido");
                    return null;
                }

                var chofer = await _context.Choferes.FindAsync(id);

                if (chofer == null)
                {
                    Console.WriteLine($"Chofer con ID {id} no encontrado");
                    return null;
                }

                return chofer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener chofer: {ex.Message}");
                return null;
            }
        }

        public async Task<Chofer?> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    Console.WriteLine("Nombre inválido");
                    return null;
                }
                var chofer = await _context.Choferes.FirstOrDefaultAsync(c => c.Nombre == nombre);
                if (chofer == null)
                {
                    Console.WriteLine($"Chofer con nombre {nombre} no encontrado");
                    return null;
                }
                return chofer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener chofer: {ex.Message}");
                return null;
            }
        }

        // UPDATE - Actualizar datos de un chofer existente
        public async Task<Chofer> ActualizarAsync(int id, string nombre)
        {
            try
            {
                if (id <= 0)
                {
                    return null;
                }

                // Verificar si el chofer existe
                var choferExistente = await _context.Choferes.FindAsync(id);

                if (choferExistente == null)
                {
                    return null;
                }

                // Actualizar propiedades
                choferExistente.Nombre = nombre;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return choferExistente;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar chofer: {ex.Message}");
                return null;
            }
        }

        // DELETE - Eliminar un chofer
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                // Buscar el chofer
                var chofer = await _context.Choferes.FindAsync(id);

                if (chofer == null)
                {
                    return false;
                }

                // Eliminar el chofer
                _context.Choferes.Remove(chofer);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar chofer: {ex.Message}");
                return false;
            }
        }
    }
}