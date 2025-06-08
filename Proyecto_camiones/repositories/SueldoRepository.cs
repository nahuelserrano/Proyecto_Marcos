using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.ViewModels;


namespace Proyecto_camiones.Presentacion.Repositories
{
    public class SueldoRepository()
    {


        private ApplicationDbContext _context = General.obtenerInstancia();

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
       

        public async Task<int> InsertarAsync(float monto, int Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta, DateOnly? fecha_pago, int idCamion)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var sueldo = new Sueldo(monto, Id_Chofer, pagadoDesde, pagadoHasta, idCamion);
                if(fecha_pago != null)
                {
                    sueldo.FechaPago = fecha_pago;
                    sueldo.Pagado = true;
                }

                await _context.Sueldos.AddAsync(sueldo);
               
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    Console.WriteLine("se insertó el registro correctamente");
                    return sueldo.Id;
                }
                Console.WriteLine("No se insertó ningún registro");
                return -1;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar pago: {ex.InnerException}");
                Console.WriteLine(ex.InnerException);
                return -1;
            }
        }

        public async Task<SueldoDTO?> PagarSueldo(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var sueldo = await _context.Sueldos.FindAsync(id);
                if (sueldo == null)
                    return null;
                sueldo.Pagado = true;
                MessageBox.Show("pagado");
                    int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    Console.WriteLine("sueldo pagado");
                    return new SueldoDTO(sueldo.Id, sueldo.Monto, sueldo.Id_Chofer, sueldo.pagadoDesde, sueldo.pagadoHasta, sueldo.FechaPago, sueldo.Pagado, sueldo.IdCamion, null);
                }
                return null;
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al pagar sueldo: {ex.Message}");
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }



        public async Task<SueldoDTO?> ObtenerPorId(int id)
        {
            Sueldo? sueldo = await _context.Sueldos.FindAsync(id);

            if (sueldo == null)
                return null;

            SueldoDTO sueldoS = new SueldoDTO(
                sueldo.Id,
                sueldo.Monto,
                sueldo.Id_Chofer,
                sueldo.pagadoDesde,
                sueldo.pagadoHasta,
                sueldo.FechaPago,
                sueldo.Pagado,
                sueldo.IdCamion,
                null
            );

            return sueldoS;
      }


        public async Task<List<SueldoDTO>?> ObtenerTodosAsync(int idCamionParametro, int? idChofer)
        {

            try
            {
                if(idChofer == null)
                {
                    return null;
                }
                List<SueldoDTO> sueldos = new List<SueldoDTO>();
                if (idChofer > -1 && idCamionParametro >-1) {
                    Console.WriteLine("hola if de tenemos chofer y camión");
                    sueldos = await _context.Sueldos
                    .Join(_context.Choferes,
                          s => s.Id_Chofer,
                          chofer => chofer.Id,
                          (s, chofer) => new { Sueldo = s, Chofer = chofer })
                    .Where(sc => sc.Sueldo.IdCamion == idCamionParametro && sc.Sueldo.Id_Chofer == idChofer)
                    .OrderByDescending(sc => sc.Sueldo.Id)
                    .Select(sc => new SueldoDTO(
                        sc.Sueldo.Id,
                        sc.Sueldo.Monto,
                        sc.Sueldo.Id_Chofer,
                        sc.Sueldo.pagadoDesde,
                        sc.Sueldo.pagadoHasta,
                        sc.Sueldo.FechaPago,
                        sc.Sueldo.Pagado,
                        sc.Sueldo.IdCamion,
                        sc.Chofer.Nombre
                    ))
                    .ToListAsync();
                    return sueldos;
            } else if(idChofer < 0 && idCamionParametro > -1)
                {
                    Console.WriteLine("Hola if de tenemos camion pero no chofer");
                    sueldos = await _context.Sueldos
                      .Join(_context.Choferes,
                            s => s.Id_Chofer,
                            chofer => chofer.Id,
                            (s, chofer) => new { Sueldo = s, Chofer = chofer })
                      .Where(sc => sc.Sueldo.IdCamion == idCamionParametro)
                      .OrderByDescending(sc => sc.Sueldo.Id)
                      .Select(sc => new SueldoDTO(
                          sc.Sueldo.Id,
                          sc.Sueldo.Monto,
                          sc.Sueldo.Id_Chofer,
                          sc.Sueldo.pagadoDesde,
                          sc.Sueldo.pagadoHasta,
                          sc.Sueldo.FechaPago,
                          sc.Sueldo.Pagado,
                          sc.Sueldo.IdCamion,
                          sc.Chofer.Nombre
                      ))
                      .ToListAsync();
                } else if(idChofer>-1 && idCamionParametro < 0)
                {
                    sueldos = await _context.Sueldos
                     .Join(_context.Choferes,
                           s => s.Id_Chofer,
                           chofer => chofer.Id,
                           (s, chofer) => new { Sueldo = s, Chofer = chofer })
                     .Where(sc => sc.Sueldo.Id_Chofer == idChofer)
                     .OrderByDescending(sc => sc.Sueldo.Id)
                     .Select(sc => new SueldoDTO(
                         sc.Sueldo.Id,
                         sc.Sueldo.Monto,
                         sc.Sueldo.Id_Chofer,
                         sc.Sueldo.pagadoDesde,
                         sc.Sueldo.pagadoHasta,
                         sc.Sueldo.FechaPago,
                         sc.Sueldo.Pagado,
                         sc.Sueldo.IdCamion,
                         sc.Chofer.Nombre
                     ))
                     .ToListAsync();
                }
                else
                {
                    return null;
                }

                    return sueldos;

        }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }

        }
       



        public async Task<bool> ActualizarAsync(int id,int id_Chofer, DateOnly? pagadoDesde = null, DateOnly? pagadoHasta = null, float? monto = null, DateOnly? FechaPago = null)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var pago = await _context.Sueldos.FindAsync(id);

                // Actualizar solo los campos proporcionados
                if (monto == null && id_Chofer == null && FechaPago == null && pagadoDesde == null && pagadoHasta == null)
                {
                    return false;
                }

                // Chequeos individuales
                if (monto != null)
                {
                    pago.Monto = monto.Value;
                }

                if (id_Chofer != null)
                {
                    pago.Id_Chofer = id_Chofer;
                }

                if (pagadoDesde != null)
                {
                    pago.pagadoDesde = pagadoDesde.Value;
                }

                if (pagadoHasta != null)
                {
                    pago.pagadoHasta = pagadoHasta.Value;
                }



                if (FechaPago != null)
                {
                    pago.FechaPago = FechaPago.Value;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar pago: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var pago = await _context.Sueldos.FindAsync(id);

                if (pago == null)
                    return false;

                _context.Sueldos.Remove(pago);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}