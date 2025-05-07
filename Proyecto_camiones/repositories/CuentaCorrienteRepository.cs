﻿using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.Presentacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Repositories
{
    public class CuentaCorrienteRepository
    {

        private readonly ApplicationDbContext _context;

        public CuentaCorrienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            try
            {
                // Intentar comprobar si la conexión a la base de datos es exitosa
                bool puedeConectar = await _context.Database.CanConnectAsync();
                Console.WriteLine("rompió acá no??");
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

        public async Task<CuentaCorriente> InsertarAsync(int? idCliente, int? idFletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            try
            {

                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }
                CuentaCorriente ultimoRegistro;
                if (idCliente != null)
                {
                    ultimoRegistro = await this._context.Cuentas.Where(c => c.IdCliente == idCliente).OrderByDescending(c => c.Fecha_factura).FirstOrDefaultAsync();
                }
                else if (idFletero != null)
                {
                    ultimoRegistro = await this._context.Cuentas.Where(c => c.IdFletero == idFletero).OrderByDescending(c => c.Fecha_factura).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }

                CuentaCorriente cuenta;
                if (ultimoRegistro == null)
                {
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda, pagado);
                }
                else
                {
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda + ultimoRegistro.Saldo_Total, pagado);
                }

                await _context.Cuentas.AddAsync(cuenta);
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return cuenta;
                }
                Console.WriteLine("No se insertó ningún registro");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public async Task<CuentaCorriente> ObtenerCuentaMasRecientePorClienteIdAsync(int clienteId)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var result = await _context.Cuentas.Where(r => r.IdCliente == clienteId).OrderByDescending(r => r.Fecha_factura).FirstOrDefaultAsync();
                if (result != null)
                {
                    CuentaCorriente cuenta = new CuentaCorriente(result.IdCliente, -1, result.Fecha_factura, result.Nro_factura, result.Adeuda, result.Pagado);
                    return cuenta;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<CuentaCorriente>> ObtenerTodosAsync()
        {
            try
            {
                if(!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                var cuentas = await _context.Cuentas.ToListAsync();
                if (cuentas != null) return cuentas;
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        internal async Task<List<CuentaCorrienteDTO>> ObtenerCuentasPorIdClienteAsync(int id)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                List<CuentaCorrienteDTO> result = new List<CuentaCorrienteDTO>();
                var cuentas = await _context.Cuentas
                            .Where(c => c.IdCliente == id)
                            .Select(c => new CuentaCorrienteDTO(

                                c.Fecha_factura,
                                c.Nro_factura,
                                c.Adeuda,
                                c.Pagado,
                                c.Saldo_Total
                            )).ToListAsync();
                return cuentas;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }

        internal async Task<List<CuentaCorrienteDTO>> ObtenerCuentasDeUnFleteroAsync(int id)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }

                List<CuentaCorrienteDTO> result = new List<CuentaCorrienteDTO>();
                var cuentas = await _context.Cuentas
                            .Where(c => c.IdFletero == id)
                            .Select(c => new CuentaCorrienteDTO(

                                c.Fecha_factura,
                                c.Nro_factura,
                                c.Adeuda,
                                c.Pagado,
                                c.Saldo_Total
                            )).ToListAsync();
                return cuentas;
            }
            catch (Exception e)
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
                var cuenta = await _context.Cuentas.FindAsync(id);
                Console.WriteLine("se encontró la cuenta corriente");

                if (cuenta == null)
                { 
                    return false;
                }

                _context.Cuentas.Remove(cuenta);

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
    }
}
