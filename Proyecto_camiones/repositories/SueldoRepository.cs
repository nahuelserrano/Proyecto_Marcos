﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


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
       

        public async Task<int> InsertarAsync(float monto, int Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta,int idCamion)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }
              
                var sueldo = new Sueldo(monto, Id_Chofer, pagadoDesde, pagadoHasta, idCamion);

                await _context.Sueldos.AddAsync(sueldo);
               
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return sueldo.Id;
                }
                Console.WriteLine("No se insertó ningún registro");
                return -1;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar pago: {ex.InnerException}");

             
                return -1;
            }
        }

        public async Task<bool> PagarSueldo(int id)
        {
            try
            {
                var sueldo = await _context.Sueldos.FindAsync(id);
                if (sueldo == null)
                    return false;
                sueldo.Pagado = true;
                sueldo.FechaPago = DateOnly.FromDateTime(DateTime.Now);
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return true;
                    Console.WriteLine("sueldo pagado");
                }
                return false;
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al pagar sueldo: {ex.Message}");
                return false;
            }
        }



        public async Task<SueldoDTO?> ObtenerPorId(int id)
        {
            Sueldo sueldo = await _context.Sueldos.FindAsync(id);

            if (sueldo == null)
                return null;

            SueldoDTO sueldoS = new SueldoDTO(
                sueldo.Monto,
                sueldo.Id_Chofer,
                sueldo.pagadoDesde,
                sueldo.pagadoHasta,
                sueldo.FechaPago,
                sueldo.Pagado,
                sueldo.IdCamion
            );

            return sueldoS;
      }


        public async Task<List<SueldoDTO>> ObtenerTodosAsync(int idCamionParametro,int idChofer) 
        {

            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }
                if (idChofer>0) {
                    List<CuentaCorrienteDTO> result1 = new List<CuentaCorrienteDTO>();
                    var sueldo1 = await _context.Sueldos
                                .Where(s => s.IdCamion == idCamionParametro && s.Id_Chofer== idChofer)
                                .OrderByDescending(c => c.Id)
                                   .Select(c => new SueldoDTO(

                                    c.Monto,
                                    c.Id_Chofer,
                                    c.pagadoDesde,
                                    c.pagadoHasta,
                                    c.FechaPago,
                                    c.Pagado,
                                    c.IdCamion
                                ))
                                .ToListAsync();
                    return sueldo1;
                }

                    List<CuentaCorrienteDTO> result = new List<CuentaCorrienteDTO>();
                    var sueldos = await _context.Sueldos
                                .Where(s => s.IdCamion == idCamionParametro)
                                .OrderByDescending(c => c.Id)
                                   .Select(c => new SueldoDTO(

                                    c.Monto,
                                    c.Id_Chofer,
                                    c.pagadoDesde,
                                    c.pagadoHasta,
                                    c.FechaPago,
                                    c.Pagado,
                                    c.IdCamion
                                ))
                                .ToListAsync();
                    return sueldos;
                
                
            }
            catch (Exception e)
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