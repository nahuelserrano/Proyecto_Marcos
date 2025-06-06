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
        private ApplicationDbContext _context;

        public ChoferRepository()
        {
            this._context = General.obtenerInstancia();
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                this._context = General.obtenerInstancia();
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
                return false;
            }
        }

        // CRUD OPERATIONS

        // CREATE - Insertar un nuevo chofer
        public async Task<int> InsertarAsync(string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                if (!await _context.Database.CanConnectAsync())
                {
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
                    return null;
                }

                var chofer = await _context.Choferes.FindAsync(id);

                if (chofer == null)
                {
                    return null;
                }

                return chofer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Chofer?> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return null;
                }
                var chofer = await _context.Choferes.FirstOrDefaultAsync(c => c.Nombre == nombre);
                if (chofer == null)
                {
                    return null;
                }
                return chofer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // UPDATE - Actualizar datos de un chofer existente
        public async Task<Chofer> ActualizarAsync(int id, string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
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
                return null;
            }
        }

        // DELETE - Eliminar un chofer
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
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
                return false;
            }
        }
    }
}