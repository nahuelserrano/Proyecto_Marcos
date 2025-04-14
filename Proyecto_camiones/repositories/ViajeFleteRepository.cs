using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Repositories
{
    public class ViajeFleteRepository
    {

        private readonly ApplicationDbContext _context;

        public ViajeFleteRepository(ApplicationDbContext context)
        {
            _context = context;
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

        internal async Task<int> InsertarViajeFlete(string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, int idCliente, int idFlete, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            try
            {
                ViajeFlete viaje = new ViajeFlete(origen, destino, remito, carga, km, kg, tarifa, factura, idCliente, idFlete, nombre_chofer, comision, fecha_salida);
                this._context.ViajesFlete.Add(viaje);
                int registros_afectados = await this._context.SaveChangesAsync();
                if (registros_afectados > 0)
                {
                    return viaje.idViajeFlete;
                }
                return -1;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
        }
    }
}
