//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using NPOI.POIFS.Properties;
//using Org.BouncyCastle.Bcpg.OpenPgp;
//using Proyecto_camiones.Presentacion.Models;

//namespace Proyecto_camiones.Presentacion.Repositories
//{
//    public class ClienteRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public ClienteRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<bool> ProbarConexionAsync()
//        {
//            try
//            {
//                // Intentar comprobar si la conexión a la base de datos es exitosa
//                bool puedeConectar = await _context.Database.CanConnectAsync();
//                Console.WriteLine("rompió acá no??");
//                if (puedeConectar)
//                {
//                    Console.WriteLine("Conexión exitosa a la base de datos.");
//                }
//                else
//                {
//                    Console.WriteLine("No se puede conectar a la base de datos.");
//                }

//                return puedeConectar;
//            }
//            catch (Exception ex)
//            {
//                // Si ocurre un error (por ejemplo, si la base de datos no está disponible)
//                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
//                return false;
//            }
//        }


//        public async Task<Cliente> ObtenerPorId(int? id)
//        {
//            try
//            {
//                var cliente = await _context.Clientes.FindAsync(id);
//                return cliente;
//            }catch(Exception e)
//            {
//                Console.WriteLine($"Stack trace: {e.StackTrace}");
//                return null;
//            }
//        }

//        public async Task<List<Cliente>> ObtenerTodos()
//        {
//            try
//            {
//                var clientes = await _context.Clientes.ToListAsync();
//                return clientes;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error al insertar camión: {ex.Message}");
//                Console.WriteLine($"Stack trace: {ex.StackTrace}");
//                return null;
//            }
//        }

//        public async Task<Cliente> InsertarAsync(string nombre)
//        {
//            try
//            {

//                if (!await _context.Database.CanConnectAsync())
//                {
//                    Console.WriteLine("No se puede conectar a la base de datos");
//                    return null;
//                }

//                var cliente = new Cliente(nombre);

//                // Agregar el camión a la base de datos (esto solo marca el objeto para insertar)
//                _context.Clientes.Add(cliente);

//                Console.WriteLine($"SQL a ejecutar: {_context.ChangeTracker.DebugView.LongView}");

//                // Guardar los cambios en la base de datos (aquí se genera el SQL real y se ejecuta)
//                int registrosAfectados = await _context.SaveChangesAsync();

//                if (registrosAfectados > 0)
//                {
//                    return cliente;
//                }
//                Console.WriteLine("No se insertó ningún registro");
//                return null;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error al insertar camión: {ex.Message}");
//                // Para debuggear, también es útil ver la excepción completa:
//                Console.WriteLine($"Stack trace: {ex.StackTrace}");
//                return null;
//            }
//        }

//        public async Task<Cliente> ActualizarAsync(int id, string? nombre)
//        {
//            var cliente = await this._context.Clientes.FindAsync(id);
//            if(cliente != null)
//            {
//                if (nombre != null)
//                {
//                    cliente.Nombre = nombre;
//                }
//                await _context.SaveChangesAsync();
//                return cliente;
//            }
//            return null;
//        }


//        public async Task<bool> Eliminar(int id)
//        {
//            try
//            {
//                var cliente = await _context.Clientes.FindAsync(id);

//                if (cliente == null)
//                    return false;

//                _context.Clientes.Remove(cliente);

//                await _context.SaveChangesAsync();

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public async Task<Cliente?> ObtenerPorNombre(string nombre_cliente)
//        {
//            try
//            {
//                  Cliente cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nombre == nombre_cliente);
//                return cliente;

//            }
//            catch(Exception e)
//            {
//                Console.WriteLine(e.Message);
//                return null;
//            }
//        }

//        //PARA QUE SE USA? NECESARIO?
//        //public async Task<List<Viaje>> ObtenerHistorialViajes(int clienteId)
//        //{
//        //    // Devolvemos una lista vacía de viajes para simular
//        //    return new List<Viaje>();
//        //}
//    }
//}