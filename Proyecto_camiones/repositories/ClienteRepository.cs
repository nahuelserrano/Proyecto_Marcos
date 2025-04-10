using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_camiones.Presentacion.Repositories
{
    public class ClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
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

        //agrego el signo de pregunta luego de Cliente para decir que el result puede ser null
        public async Task<Cliente?> InsertarClienteAsync(string nombre, string apellido, string dni)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var cliente = new Cliente(nombre, apellido, dni)
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Dni = dni
                };

                // Agregar el cliente a la base de datos (esto solo marca el objeto para insertar)
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
                Console.WriteLine($"Error al insertar cliente: {ex.Message}");
                // Para debuggear, también es útil ver la excepción completa:
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<Cliente>?> ObtenerClientesAsync()
        {
            try
            {
                var clientes = await _context.Clientes.Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Dni = c.Dni
                }).ToListAsync();

                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<bool> Actualizar(int id, string? nombre=null, string? apellido = null, string? dni = null)   
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                    return false;

                if (!string.IsNullOrEmpty(nombre))
                {
                    cliente.Nombre = nombre;
                }

                if (!string.IsNullOrEmpty(apellido))
                {
                    cliente.Apellido = apellido;
                }

                if (!string.IsNullOrEmpty(dni))
                {
                    cliente.Dni = dni;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                return false;
            }
        }

        internal async Task<ClienteDTO?> ObtenerPorId(int id)
        {
            try
            {
                Cliente cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    ClienteDTO nuevo = new ClienteDTO
                    {
                        Id = cliente.Id,
                        Nombre = cliente.Nombre,
                        Apellido = cliente.Apellido,
                        Dni = cliente.Dni
                    };
                    return nuevo;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cliente por ID: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                    return false;

                _context.Clientes.Remove(cliente);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<int> ObtenerIdCamion(int clienteId)
        {
            try
            {
                var cliente = await _context.Clientes
                    .Include(c => c.Camion)
                    .FirstOrDefaultAsync(c => c.Id == clienteId);

                return cliente?.Camion?.Id ?? -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ID del camión: {ex.Message}");
                return -1;
            }
        }

        public async Task<List<ViajeDTO>?> ObtenerHistorialViajes(int clienteId)
        {
            try
            {
                var viajes = await _context.Viajes
                    .Where(v => v.ClienteId == clienteId)
                    .Select(v => new ViajeDTO
                    {
                        Id = v.Id,
                        FechaSalida = v.FechaSalida,
                        FechaLlegada = v.FechaLlegada,
                        Origen = v.Origen,
                        Destino = v.Destino,
                        ClienteId = v.ClienteId
                    })
                    .ToListAsync();

                return viajes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener historial de viajes: {ex.Message}");
                return null;
            }
        }
    }
}