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
                this._context = General.obtenerInstancia();
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
                var chofer = await _context.Choferes.FirstOrDefaultAsync(c => c.Nombre == nombre);

                if (chofer == null)
                {
                    var match = await ObtenerPorSimilitudAsync(nombre);

                    chofer = match.Value.chofer;
                    double similitud = match.Value.similitud;

                    if (chofer == null || similitud < 75.0)
                    {
                        Console.WriteLine($"Chofer con nombre {nombre} no encontrado");
                        return null;
                    }
                    Console.WriteLine($"Chofer con nombre {nombre} no encontrado, Match {chofer.Nombre} con una similitud de {similitud}");
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
                Console.WriteLine($"Error al actualizar chofer: {ex.Message}");
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
                Console.WriteLine($"Error al eliminar chofer: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Búsqueda híbrida: Combina LIKE y similitud calculada en C#
        /// Mejor balance entre performance y precisión
        /// </summary>
        public async Task<(Chofer chofer, double similitud)?> ObtenerPorSimilitudAsync(string nombreBuscado, double umbralMinimo = 75.0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreBuscado))
                    return null;

                // Paso 1: Filtrado rápido con LIKE (reduce el dataset)
                var candidatos = await BuscarChoferConLikeAsync(nombreBuscado);

                if (!candidatos.Any())
                {
                    // Paso 2: Si no hay resultados con LIKE, buscar en todos
                    candidatos = await _context.Choferes.ToListAsync();
                }

                // Paso 3: Calcular similitud en memoria (solo para candidatos filtrados)
                var mejorMatch = candidatos
                    .Select(c => new {
                        Chofer = c,
                        Similitud = CalcularSimilitudSimple(nombreBuscado, c.Nombre)
                    })
                    .Where(x => x.Similitud >= umbralMinimo)
                    .OrderByDescending(x => x.Similitud)
                    .FirstOrDefault();

                return mejorMatch != null ? (mejorMatch.Chofer, mejorMatch.Similitud) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en búsqueda híbrida: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Cálculo de similitud simple y rápido (más eficiente que Levenshtein completo)
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

        /// <summary>
        /// Busca choferes usando LIKE pattern matching - Simple y efectivo
        /// </summary>
        public async Task<List<Chofer>> BuscarChoferConLikeAsync(string nombreBuscado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreBuscado))
                    return new List<Chofer>();

                // Crear patrones de búsqueda flexibles
                string patron1 = $"%{nombreBuscado}%";
                string patron2 = $"{nombreBuscado}%";
                string patron3 = $"%{nombreBuscado}";

                var choferes = await _context.Choferes
                    .Where(c => EF.Functions.Like(c.Nombre, patron1) ||
                                EF.Functions.Like(c.Nombre, patron2) ||
                                EF.Functions.Like(c.Nombre, patron3))
                    .ToListAsync();

                return choferes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en búsqueda LIKE: {ex.Message}");
                return new List<Chofer>();
            }
        }

    }
}