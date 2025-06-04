using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Repositories
{
    public class FleteRepository
    {
        private ApplicationDbContext _context;

        public FleteRepository()
        {
            this._context = General.obtenerInstancia();
        }

        public async Task<bool> ProbarConexionAsync()
        {
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
                Console.WriteLine($"Error al intentar conectar: {ex.Message}");
                return false;
            }
        }

        public async Task<int> InsertarAsync(string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                Flete nuevo = new Flete(nombre);
                await this._context.Fletes.AddAsync(nuevo);
                int registrosAfectados = await this._context.SaveChangesAsync();
                Console.WriteLine($"Registros afectados: {registrosAfectados}");
                if (registrosAfectados > 0)
                {
                    return nuevo.Id;
                }
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return -1;
            }
        }

        internal async Task<Flete> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                Flete flete = await _context.Fletes.Where(f => f.nombre == nombre).FirstOrDefaultAsync();
                return flete;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }

        internal async Task<List<Flete>> ObtenerTodosAsync()
        {
            try
            {
                List<Flete> fleteros = await this._context.Fletes.OrderByDescending(f=> f.Id).ToListAsync();
                return fleteros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }

        internal async Task<Flete> ObtenerPorIdAsync(int id)
        {
            try
            {
                Flete fletero = await this._context.Fletes.FindAsync(id);
                return fletero;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }

        internal async Task<bool> EliminarAsync(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var fletero = await _context.Fletes.FindAsync(id);
                MessageBox.Show("se encontró el fletero");

                if (fletero == null)
                    return false;

                _context.Fletes.Remove(fletero);

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

        internal async Task<Flete> ActualizarAsync(int id, string nombre)
        {
            try
            {
                this._context = General.obtenerInstancia();
                Flete? fletero = await this._context.Fletes.FindAsync(id);
                if (fletero == null) return null;
                fletero.nombre = nombre;
                int registros_afectados = await this._context.SaveChangesAsync();
                if(registros_afectados > 0)
                {
                    return fletero;
                }
                return null;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
    }
}
