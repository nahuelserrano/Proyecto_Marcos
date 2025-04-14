using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;


namespace Proyecto_camiones.Presentacion.Repositories
{
    public class PagoRepository
    {
        private List<Pago> _pagos;
        private int _siguienteId;
        private readonly ApplicationDbContext _context;
        public PagoRepository(ApplicationDbContext context)
        {
            this._context = context;
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


        public async Task<int> Insertar(float monto, int Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta, DateOnly FechaPago)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }
              
                var pago = new Pago(monto, Id_Chofer, pagadoDesde, pagadoHasta, FechaPago);


                _context.Pagos.Add(pago);

                await _context.SaveChangesAsync();
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return 1;
                }
                Console.WriteLine("No se insertó ningún registro");
                return -1;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar pago: {ex.Message}");

                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1;
            }
        }



      public async Task<PagoDTO?> ObtenerPorId(int id)
      {
            Pago pago= await _context.Pagos.FindAsync(id);
    
            if (pago == null)
                return null;

            PagoDTO pagop = new PagoDTO(
                pago.Monto_Pagado,
                pago.Id_Chofer,
                pago.pagadoDesde,
                pago.pagadoHasta,
                pago.FechaDePago
            );

            return pagop;
      }

       
        public async Task<List<PagoDTO>> ObtenerTodos()
        {
            
                var pagos = await _context.Pagos.Select(p => new PagoDTO
                {
                    Monto_Pagado = p.Monto_Pagado,
                    Id_Chofer = p.Id_Chofer,
                    pagadoDesde = p.pagadoDesde,
                    pagadoHasta = p.pagadoHasta,
                    FechaDePago = p.FechaDePago

                }).ToListAsync();

                return pagos;
            
        }


        public async Task<bool> Actualizar(int id,int id_Chofer, DateOnly? pagadoDesde = null, DateOnly? pagadoHasta = null, float? monto = null, DateOnly? FechaPago = null)
        {
            try
            {
                var pago = await _context.Pagos.FindAsync(id);

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
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var pago = await _context.Pagos.FindAsync(id);

                if (pago == null)
                    return false;

                _context.Pagos.Remove(pago);

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