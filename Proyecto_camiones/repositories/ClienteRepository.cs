using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Properties;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ClienteRepository
    {
        private ApplicationDbContext _context;

        public ClienteRepository()
        {
            this._context = General.obtenerInstancia();
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                Console.WriteLine("Iniciando prueba de conexión...");
                // Verificamos si el contexto existe
                if (_context == null)
                {
                    Console.WriteLine("ERROR: _context es NULL");
                    return false;
                }

                Console.WriteLine("Accediendo a Database...");
                // Verificamos si la propiedad Database existe
                if (_context.Database == null)
                {
                    Console.WriteLine("ERROR: _context.Database es NULL");
                    return false;
                }

                Console.WriteLine("Llamando a CanConnectAsync...");
                // Aquí es donde parece detenerse la ejecución
                bool puedeConectar = await _context.Database.CanConnectAsync();

                Console.WriteLine("Resultado de conexión: " + puedeConectar);
                return puedeConectar;
                /*
                // Intentar comprobar si la conexión a la base de datos es exitosa
                Console.WriteLine("Probar conexión");
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
                */
            }
            catch (Exception ex)
            {
                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }


        public async Task<Cliente?> ObtenerPorIdAsync(int? id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                    return null;
                
                return cliente;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Stack trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            try
            {
                var clientes = await _context.Clientes.OrderByDescending(c => c.Id).ToListAsync();
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar camión: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<Cliente> InsertarAsync(string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var cliente = new Cliente(nombre);

                // Agregar el camión a la base de datos (esto solo marca el objeto para insertar)
                _context.Clientes.Add(cliente);

                Console.WriteLine($"SQL a ejecutar: {_context.ChangeTracker.DebugView.LongView}");

                // Guardar los cambios en la base de datos (aquí se genera el SQL real y se ejecuta)
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return cliente;
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

        public async Task<Cliente> ActualizarAsync(int id, string? nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                Cliente? cliente = await this._context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    if (nombre != null)
                    {
                        cliente.Nombre = nombre;
                    }
                    int registros_afectados = await _context.SaveChangesAsync();
                    if(registros_afectados > 0)
                    {
                        return cliente;
                    }
                }
                return null;
            }
            catch(Exception e)
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
                this._context = General.obtenerInstancia();
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                    return false;

                _context.Clientes.Remove(cliente);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Cliente?> ObtenerPorNombreAsync(string nombre_cliente)
        {
            try
            {
                Cliente? cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nombre == nombre_cliente);
                return cliente;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}