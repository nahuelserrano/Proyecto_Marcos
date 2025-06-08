using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion.Repositories
{

    public class ChequeRepository
    {
        private ApplicationDbContext _context;


        public ChequeRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                this._context = General.obtenerInstancia();
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

        public async Task<int> InsertarAsync(
            DateOnly fechaIngreso,
            int numeroCheque,
            float monto,
            string banco,
            DateOnly fechaCobro,
            string nombre = "",
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null,
            string entregadoA = "") // NUEVO PARÁMETRO - Más importante que saber el verdadero nombre de McLovin
        {
            try
            {
                this._context = General.obtenerInstancia();
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return -1;
                }

                var cheque = new Cheque
                {
                    FechaIngresoCheque = fechaIngreso,
                    NumeroCheque = numeroCheque,
                    Monto = monto,
                    Banco = banco,
                    FechaCobro = fechaCobro,
                    Nombre = nombre,
                    NumeroPersonalizado = numeroPersonalizado,
                    FechaVencimiento = fechaVencimiento,
                    EntregadoA = entregadoA // ASIGNAR NUEVO CAMPO
                };

                _context.Cheques.Add(cheque);
                int registrosAfectados = await _context.SaveChangesAsync();

                Console.WriteLine($"Registros afectados: {registrosAfectados}");

                if (registrosAfectados > 0)
                    return cheque.Id;

                Console.WriteLine("No se insertó ningún registro");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar cheque: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return -1;
            }
        }

        public async Task<ChequeDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                Cheque? cheque = await _context.Cheques
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (cheque != null)
                {
                    ChequeDTO nuevo = new ChequeDTO
                    {
                        Id = cheque.Id,
                        FechaIngresoCheque = cheque.FechaIngresoCheque,
                        NumeroCheque = cheque.NumeroCheque,
                        Monto = cheque.Monto,
                        Banco = cheque.Banco,
                        FechaCobro = cheque.FechaCobro,
                        Nombre = cheque.Nombre,
                        NumeroPersonalizado = cheque.NumeroPersonalizado,
                        FechaVencimiento = cheque.FechaVencimiento,
                        EntregadoA = cheque.EntregadoA // MAPEAR NUEVO CAMPO
                    };
                    return nuevo;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cheque con ID: {id}, exception: {ex}");
                return null;
            }
        }

        public async Task<List<ChequeDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var cheques = await _context.Cheques.Select(c => new ChequeDTO
                {
                    Id = c.Id,
                    FechaIngresoCheque = c.FechaIngresoCheque,
                    NumeroCheque = c.NumeroCheque,
                    Monto = c.Monto,
                    Banco = c.Banco,
                    FechaCobro = c.FechaCobro,
                    Nombre = c.Nombre,
                    NumeroPersonalizado = c.NumeroPersonalizado,
                    FechaVencimiento = c.FechaVencimiento,
                    EntregadoA = c.EntregadoA // MAPEAR NUEVO CAMPO
                }).ToListAsync();

                return cheques;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                throw;
            }
        }

        public async Task<bool> ActualizarAsync(
            int id,
            DateOnly? fechaIngreso = null,
            int? numeroCheque = null,
            float? monto = null,
            string? banco = null,
            DateOnly? fechaCobro = null,
            string? nombre = null,
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null,
            string? entregadoA = null) // NUEVO PARÁMETRO
        {
            try
            {
                this._context = General.obtenerInstancia();
                var cheque = await _context.Cheques.FindAsync(id);

                if (cheque == null)
                {
                    Console.WriteLine($"Cheque con ID {id} no encontrado.");
                    return false;
                }

                // Actualizar solo los campos proporcionados
                if (fechaIngreso.HasValue)
                    cheque.FechaIngresoCheque = fechaIngreso.Value;

                if (numeroCheque.HasValue)
                    cheque.NumeroCheque = numeroCheque.Value;

                if (monto.HasValue)
                    cheque.Monto = monto.Value;

                if (!string.IsNullOrEmpty(banco))
                    cheque.Banco = banco;

                if (fechaCobro.HasValue)
                    cheque.FechaCobro = fechaCobro.Value;

                if (nombre != null)
                    cheque.Nombre = nombre;

                if (numeroPersonalizado.HasValue)
                    cheque.NumeroPersonalizado = numeroPersonalizado;

                if (fechaVencimiento.HasValue)
                    cheque.FechaVencimiento = fechaVencimiento.Value;

                if (entregadoA != null) // ACTUALIZAR NUEVO CAMPO
                    cheque.EntregadoA = entregadoA;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cheque: {ex.Message}");
                return false;
            }
        }

        public async Task<ChequeDTO?> ObtenerPorNumeroChequeAsync(int nroCheque)
        {
            try
            {
                Cheque? cheque = await _context.Cheques
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.NumeroCheque == nroCheque);

                if (cheque != null)
                {
                    ChequeDTO nuevo = new ChequeDTO
                    {
                        Id = cheque.Id,
                        FechaIngresoCheque = cheque.FechaIngresoCheque,
                        NumeroCheque = cheque.NumeroCheque,
                        Monto = cheque.Monto,
                        Banco = cheque.Banco,
                        FechaCobro = cheque.FechaCobro,
                        Nombre = cheque.Nombre,
                        NumeroPersonalizado = cheque.NumeroPersonalizado,
                        FechaVencimiento = cheque.FechaVencimiento,
                        EntregadoA = cheque.EntregadoA // MAPEAR NUEVO CAMPO
                    };
                    return nuevo;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cheque con numero de cheque: {nroCheque}, exception: {ex}");
                return null;
            }
        }


        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                this._context = General.obtenerInstancia();
                Cheque? cheque = await _context.Cheques.FindAsync(id);

                if (cheque == null)
                    return false;

                _context.Cheques.Remove(cheque);
                int registrosAfectados = await _context.SaveChangesAsync();
                return registrosAfectados > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Repository: Error al eliminar cheque con ID: {id}");
                Console.WriteLine(e.InnerException != null ? e.InnerException.Message : e.Message);
                return false;
            }
        }
    }
}