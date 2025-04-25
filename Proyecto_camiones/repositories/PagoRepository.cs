using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Proyecto_camiones.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_camiones.Repositories
{
    public class PagoRepository
    {
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
    
       public async Task<int> Insertar(int id_chofer, int id_viaje,  float monto_pagado)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }
                var pago = new Pago( id_chofer, id_viaje,monto_pagado);

                _context.Pagos.Add(pago);
                int registrosAfectados = await this._context.SaveChangesAsync();
                Console.WriteLine($"Registros afectados: {registrosAfectados}");
                if (registrosAfectados > 0)
                {
                    return pago.Id;
                }
                return -2;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al insertar el pago: {e.InnerException}");
                return -3;
            }
        }



        public async Task<bool> Actualizar(int? id=null,int? id_Sueldo=null,int?id_viaje=null, float? monto_pagado_nuevo = null)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return false;
                }
                if (id_viaje != null && monto_pagado_nuevo!=null)
                {
                    Pago pago = await _context.Pagos.FindAsync(id_viaje);
                    if (pago == null)
                    {
                        return false;
                    }
                    pago.Monto_Pagado = monto_pagado_nuevo.Value;
                    return true;
                }


                if (id != null&&id_Sueldo!=null)
                {
                    Pago pagoExistente = await _context.Pagos.FindAsync(id);
                    if (pagoExistente == null)
                    {
                        return false;
                    }
                    pagoExistente.Pagado = true;
                    pagoExistente.Id_sueldo = id_Sueldo;
                    return true;

                }




                int registrosAfectados = this._context.SaveChanges();
                if (registrosAfectados > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public async Task<List<Pago>> ObtenerPagosAsync(int idChofer, DateOnly fechaDesde, DateOnly fechaHasta)
        {
            try
            {
                var pagosPorViajeEnRango = await _context.Pagos
                    .Where(pago => pago.Id_Chofer == idChofer && pago.Pagado == false) // Filtrar pagos por el Id_Viaje y ver si esta pago o no
                    .Join(
                        _context.Viajes,
                        pago => pago.Id_Viaje,
                        viaje => viaje.Id,
                        (pago, viaje) => new { Pago = pago, Viaje = viaje }
                    )
                    .Where(joinResult => joinResult.Viaje.FechaInicio >= fechaDesde && joinResult.Viaje.FechaInicio <= fechaHasta)
                    .Select(joinResult => joinResult.Pago) // Seleccionamos solo los objetos Pago resultantes
                    .ToListAsync();

                return pagosPorViajeEnRango;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los pagos: {ex.Message}");
                return new List<Pago>();
            }
        }

    }

}
