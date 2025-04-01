using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ChoferRepository
    {
        private readonly ApplicationDbContext _context;

        public ChoferRepository(ApplicationDbContext context)
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

        // CRUD OPERATIONS

        // CREATE - Insertar un nuevo chofer
        public async Task<int> Insertar(string nombre, string apellido)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1; // Mejor retornar un valor específico de error que null
                }

                var chofer = new Chofer(nombre, apellido);

                _context.Choferes.Add(chofer);

                await _context.SaveChangesAsync();

                return chofer.Id; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar chofer: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1; // Mejor retornar un valor específico de error
            }
        }

        // READ - Obtener todos los choferes
        public async Task<List<Chofer>> ObtenerChoferes()
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
        public async Task<Chofer> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID inválido");
                    return null;
                }

                return await _context.Choferes.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener chofer: {ex.Message}");
                return null;
            }
        }

        // UPDATE - Actualizar datos de un chofer existente
        public async Task<bool> Actualizar(int id, string nombre, string apellido)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                // Verificar si el chofer existe
                var choferExistente = await _context.Choferes.FindAsync(id);
                if (choferExistente == null)
                {
                    return false;
                }

                // Actualizar propiedades
                choferExistente.nombre = nombre;
                choferExistente.apellido = apellido;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar chofer: {ex.Message}");
                return false;
            }
        }

        // DELETE - Eliminar un chofer
        public async Task<bool> Eliminar(int id)
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

        // MÉTODOS ESPECÍFICOS PARA CHOFER
    }
}