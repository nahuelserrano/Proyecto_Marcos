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
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.ViewModels;
using System.Windows.Forms;

namespace Proyecto_camiones.Repositories
{
    public class PagoRepository
    {
        private ApplicationDbContext _context;

        public PagoRepository()
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

        public async Task<int> InsertarAsync(int id_chofer, int id_viaje,  float monto_pagado)
        {
            try
            {
                this._context = General.obtenerInstancia();
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

        public async Task<bool> modificarEstado(int idChofer, DateOnly desde, DateOnly hasta, int? id_Sueldo,bool pagado)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var pagosModificar = await _context.Pagos
                .Where(pago => pago.Id_Chofer == idChofer && pago.Pagado  ==  !pagado) // Filtrar pagos por el Id_Viaje y ver si esta pago o no
                .Join(
                    _context.Viajes,
                    pago => pago.Id_Viaje,
                    viaje => viaje.Id,
                    (pago, viaje) => new { Pago = pago, Viaje = viaje }
                )
                .Where(joinResult => joinResult.Viaje.FechaInicio >= desde && joinResult.Viaje.FechaInicio <= hasta)
                .Select(joinResult => joinResult.Pago) // Seleccionamos solo los objetos Pago resultantes
                .ToListAsync();

                if (pagosModificar.Any())
                {
                    foreach (var pago in pagosModificar)
                    {
                        pago.Pagado = pagado;
                        if(id_Sueldo!=null)
                        pago.Id_sueldo = id_Sueldo;
                    }

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

        public async Task<bool> ActualizarAsync(int id_chofer, int id_viaje, float monto_pagado)
        {
            try
            {
                this._context = General.obtenerInstancia();
                var pago = await _context.Pagos
                    .Where(p => p.Id_Viaje == id_viaje)
                    .FirstOrDefaultAsync();

                if (pago != null)
                {
                    if (!pago.Pagado) {

                        Console.WriteLine($"📊 Pago encontrado - ID: {pago.Id}, Chofer actual: {pago.Id_Chofer}, Pagado: {pago.Pagado}");

                        if (pago.Id_Chofer != id_chofer)
                            pago.Id_Chofer = id_chofer;
                        
                        if(pago.Monto_Pagado!=monto_pagado)
                            pago.Monto_Pagado = monto_pagado;

                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al actualizar el pago: {e.Message}");
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
                MessageBox.Show(" " + pagosPorViajeEnRango);
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
