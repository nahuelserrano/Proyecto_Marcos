using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                bool puedeConectar = await _context.Database.CanConnectAsync();
                return puedeConectar;
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
                this._context = General.obtenerInstancia();
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
                this._context = General.obtenerInstancia();
                var clientes = await _context.Clientes.OrderByDescending(c => c.Id).ToListAsync();
                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<Cliente> InsertarAsync(string nombre)
        {
            try
            {
                this._context = General.obtenerInstanciaTemporal();
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
                Console.WriteLine($"Error al insertar cliente: {ex.Message}");
                // Para debuggear, también es útil ver la excepción completa:
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<Cliente> ActualizarAsync(int id, string? nombre)
        {
            try
            {
                this._context = General.obtenerInstanciaTemporal();
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
                this._context = General.obtenerInstanciaTemporal();
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
                this._context = General.obtenerInstancia();
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nombre == nombre_cliente);

                if (cliente == null)
                {
                    var match = await ObtenerPorSimilitudAsync(nombre_cliente);

                    if (match.HasValue && match.Value.similitud >= 75.0)
                    {
                        cliente = match.Value.cliente;
                        double similitud = match.Value.similitud;

                        Console.WriteLine(
                            $"Cliente con nombre {nombre_cliente} no encontrado, Match {cliente.Nombre} con una similitud de {similitud}");
                    }
                    else
                    {
                        Console.WriteLine($"Cliente con nombre {nombre_cliente} no encontrado");
                        return null;
                    }
                }

                return cliente;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        #region Métodos Fuzzy Matching para Clientes

        /// <summary>
        /// Búsqueda híbrida para clientes: Combina LIKE y similitud calculada en C#
        /// Mismo patrón que ChoferRepository.ObtenerPorSimilitudAsync()
        /// </summary>
        public async Task<(Cliente cliente, double similitud)?> ObtenerPorSimilitudAsync(string nombreBuscado, double umbralMinimo = 75.0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreBuscado))
                    return null;

                // Paso 1: Filtrado rápido con LIKE (reduce el dataset)
                var candidatos = await BuscarClienteConLikeAsync(nombreBuscado);

                if (!candidatos.Any())
                {
                    // Paso 2: Si no hay resultados con LIKE, buscar en todos
                    candidatos = await _context.Clientes.ToListAsync();
                }

                // Paso 3: Calcular similitud en memoria (solo para candidatos filtrados)
                var mejorMatch = candidatos
                    .Select(c => new {
                        Cliente = c,
                        Similitud = CalcularSimilitudSimple(nombreBuscado, c.Nombre)
                    })
                    .Where(x => x.Similitud >= umbralMinimo)
                    .OrderByDescending(x => x.Similitud)
                    .FirstOrDefault();

                return mejorMatch != null ? (mejorMatch.Cliente, mejorMatch.Similitud) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en búsqueda híbrida de cliente: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca clientes usando LIKE pattern matching - Simple y efectivo
        /// Mismo patrón que ChoferRepository.BuscarChoferConLikeAsync()
        /// </summary>
        public async Task<List<Cliente>> BuscarClienteConLikeAsync(string nombreBuscado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreBuscado))
                    return new List<Cliente>();

                // Crear patrones de búsqueda flexibles
                string patron1 = $"%{nombreBuscado}%";
                string patron2 = $"{nombreBuscado}%";
                string patron3 = $"%{nombreBuscado}";

                var clientes = await _context.Clientes
                    .Where(c => EF.Functions.Like(c.Nombre, patron1) ||
                               EF.Functions.Like(c.Nombre, patron2) ||
                               EF.Functions.Like(c.Nombre, patron3))
                    .ToListAsync();

                return clientes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en búsqueda LIKE de cliente: {ex.Message}");
                return new List<Cliente>();
            }
        }

        /// <summary>
        /// Cálculo de similitud simple y rápido para clientes
        /// Mismo algoritmo que ChoferRepository.CalcularSimilitudSimple()
        /// </summary>
        private double CalcularSimilitudSimple(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                return 0.0;

            s1 = s1.ToLower();
            s2 = s2.ToLower();

            // Coincidencia exacta
            if (s1 == s2) return 100.0;

            // Conteo de caracteres comunes
            var caracteresComunes = s1.Intersect(s2).Count();
            var maxLength = Math.Max(s1.Length, s2.Length);

            return (double)caracteresComunes / maxLength * 100.0;
        }

        #endregion
    }
}