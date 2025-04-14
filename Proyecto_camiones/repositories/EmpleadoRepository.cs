using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Proyecto_camiones.Models;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class EmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
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
        public async Task<int> InsertarEmpleadoAsync(string nombre)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1; // Mejor retornar un valor específico de error que null
                }

                var empleado = new Empleado(nombre);

                _context.Empleados.Add(empleado);

                await _context.SaveChangesAsync();

                return empleado.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar chofer: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1; // Mejor retornar un valor específico de error
            }
        }

        // READ - Obtener todos los choferes
        public async Task<List<Empleado>> ObtenerEmpleadosAsync()
        {
            try
            {
                var empleados = await _context.Empleados.ToListAsync();
                return empleados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleados: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Empleado>();
            }
        }

        // READ - Obtener un chofer específico por ID
        public async Task<Empleado> ObtenerPorIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID inválido");
                    return null;
                }

                var empleado = await _context.Empleados.FindAsync(id);
                return empleado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado: {ex.Message}");
                return null;
            }
        }

        // UPDATE - Actualizar datos de un chofer existente
        public async Task<bool> ActualizarEmpleadoAsync(int id, string nombre)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                // Verificar si el empleado existe
                var empleadoExistente = await _context.Empleados.FindAsync(id);
                if (empleadoExistente == null)
                {
                    return false;
                }

                // Actualizar propiedades
                empleadoExistente.nombre = nombre;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar empleado: {ex.Message}");
                return false;
            }
        }

        // DELETE - Eliminar un chofer
        public async Task<bool> EliminarEmpleadoAsyn(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                // Buscar el empleado
                var empleado = await _context.Empleados.FindAsync(id);
                if (empleado == null)
                {
                    return false;
                }

                // Eliminar el empleado
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                return false;
            }
        }

        // MÉTODOS ESPECÍFICOS PARA CHOFER
    }
}