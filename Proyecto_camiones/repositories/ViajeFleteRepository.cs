using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.ViewModels;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Repositories
{
    public class ViajeFleteRepository
    {

        private readonly ApplicationDbContext _context;

        public ViajeFleteRepository()
        {
            _context = General.obtenerInstancia();
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
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }

        internal async Task<ViajeFlete> ActualizarAsync(int id, string? origen, string? destino, float? remito, string? carga, float? km, float? kg, float? tarifa, int? factura, int? clienteId, string? nombre_chofer, float? comision, DateOnly? fecha_salida)
        {
            try
            {
                var viaje = await this._context.ViajesFlete.FindAsync(id);
                if(viaje == null)
                {
                    return null;
                }
                if(origen != null)
                {
                    viaje.origen = origen;
                }
                if(destino != null)
                {
                    viaje.destino = destino;
                }
                if(remito != null)
                {
                    viaje.remito = (float)remito;
                }
                if(carga != null)
                {
                    viaje.carga = carga;
                }
                if(km != null)
                {
                    viaje.km = (float)km;
                }
                if (kg != null)
                {
                    viaje.kg = (float)kg;
                }
                if(tarifa != null)
                {
                    viaje.tarifa = (float)tarifa;
                }
                if(factura != null)
                {
                    viaje.factura = (int)factura;
                }
                if(clienteId != null)
                {
                    viaje.idCliente = (int)clienteId;
                }
                if(nombre_chofer != null)
                {
                    viaje.nombre_chofer = nombre_chofer;
                }
                if(comision != null)
                {
                    viaje.comision = (float)comision;
                }
                if(fecha_salida != null)
                {
                    viaje.fecha_salida = (DateOnly)fecha_salida;
                }

                await _context.SaveChangesAsync();
                return viaje;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }

        internal async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var viaje = await _context.ViajesFlete.FindAsync(id);
                Console.WriteLine("se encontró el fletero");

                if (viaje == null)
                    return false;

                _context.ViajesFlete.Remove(viaje);

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

        internal async Task<int> InsertarAsync(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, int idCliente, int idFlete, string? nombre_chofer, float comision, DateOnly fecha_salida)
        {
            try
            {
                ViajeFlete viaje = new ViajeFlete(origen, destino, remito, carga, km, kg, tarifa, factura, idCliente, idFlete, nombre_chofer, comision, fecha_salida);
                this._context.ViajesFlete.Add(viaje);
                int registros_afectados = await this._context.SaveChangesAsync();
                MessageBox.Show(registros_afectados + " ");
                if (registros_afectados > 0)
                {
                    return viaje.idViajeFlete;
                }
                return -1;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return -1;
            }
        }

        internal async Task<List<ViajeMixtoDTO>> ObtenerViajesDeUnClienteAsync(int id)
        {
            try
            {

                var viajes = await (from v in this._context.ViajesFlete
                                    join f in this._context.Fletes on v.idFlete equals f.Id
                                    where v.idCliente == id
                                    select new ViajeMixtoDTO(
                                        v.origen,
                                        v.destino,
                                         v.fecha_salida,
                                        v.remito,
                                         v.nombre_chofer,
                                         v.carga,
                                         v.km,
                                        v.kg,
                                         v.tarifa,
                                        v.comision,
                                        f.nombre // Aquí obtenemos el nombre del fletero
                                    )).OrderByDescending(v => v.Id)
                                    .ToListAsync();
                return viajes;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }

        internal async Task<List<ViajeFleteDTO>> ObtenerViajesPorIdFleteroAsync(int idFletero)
        {
            try
            {
                var viajes = await _context.ViajesFlete
                .Where(v => v.idFlete == idFletero)  // Filtrar por el fletero solicitado
                .Join(
                    _context.Clientes,
                    viaje => viaje.idCliente,
                    cliente => cliente.Id,  // Asumiendo que el ID del cliente es 'Id'
                    (viaje, cliente) => new { Viaje = viaje, Cliente = cliente }
                )
                .Join(
                    _context.Fletes,
                    vc => vc.Viaje.idFlete,
                    flete => flete.Id,  // Asumiendo que el ID del fletero es 'Id'
                    (vc, flete) => new ViajeFleteDTO
                    {
                        idViajeFlete = vc.Viaje.idViajeFlete,
                        origen = vc.Viaje.origen,
                        destino = vc.Viaje.destino,
                        remito = vc.Viaje.remito,
                        carga = vc.Viaje.carga,
                        km = vc.Viaje.km,
                        kg = vc.Viaje.kg,
                        tarifa = vc.Viaje.tarifa,
                        factura = vc.Viaje.factura,
                        cliente = vc.Cliente.Nombre,  // Nombre del cliente obtenido por join
                        fletero = flete.nombre,       // Nombre del fletero obtenido por join
                        nombre_chofer = vc.Viaje.nombre_chofer,
                        comision = vc.Viaje.comision,
                        fecha_salida = vc.Viaje.fecha_salida
                    }
                )
                .OrderByDescending(v => v.idViajeFlete)
                .ToListAsync();

                return viajes;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
    }
}
