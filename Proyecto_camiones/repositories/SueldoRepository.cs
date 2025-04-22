using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;


namespace Proyecto_camiones.Presentacion.Repositories
{
    public class SueldoRepository(ApplicationDbContext context)
    {
    
 
        private readonly ApplicationDbContext _context = context;

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
              
                var pago = new Sueldo(monto, Id_Chofer, pagadoDesde, pagadoHasta, FechaPago);


                _context.Sueldos.Add(pago);

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



      public async Task<SueldoDTO?> ObtenerPorId(int id)
      {
            Sueldo sueldo = await _context.Sueldos.FindAsync(id);
    
            if (sueldo == null)
                return null;

            SueldoDTO sueldoS = new SueldoDTO(
                sueldo.Monto_Pagado,
                sueldo.Id_Chofer,
                sueldo.pagadoDesde,
                sueldo.pagadoHasta,
                sueldo.FechaPago
            );

            return sueldoS;
      }

       
        public async Task<List<SueldoDTO>> ObtenerTodos()
        {
            
                var sueldo = await _context.Sueldos.Select(p => new SueldoDTO
                {
                    Monto_Pagado = p.Monto_Pagado,
                    Id_Chofer = p.Id_Chofer,
                    pagadoDesde = p.pagadoDesde,
                    pagadoHasta = p.pagadoHasta,
                    FechaDePago = p.FechaPago

                }).ToListAsync();

                return sueldo;
            
        }


        public async Task<bool> Actualizar(int id,int id_Chofer, DateOnly? pagadoDesde = null, DateOnly? pagadoHasta = null, float? monto = null, DateOnly? FechaPago = null)
        {
            try
            {
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
        public async Task<bool> Eliminar(int id)
        {
            try
            {
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