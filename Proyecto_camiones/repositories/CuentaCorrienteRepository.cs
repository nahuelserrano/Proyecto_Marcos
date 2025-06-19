﻿using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Repositories
{
    public class CuentaCorrienteRepository
    {

        private ApplicationDbContext _context;

        public CuentaCorrienteRepository()
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

        public async Task<CuentaCorriente> InsertarAsync(int? idCliente, int? idFletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    return null;
                }
                CuentaCorriente ultimoRegistro;
                if (idCliente != null)
                {
                    ultimoRegistro = await this._context.Cuentas.Where(c => c.IdCliente == idCliente).OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                }
                else if (idFletero != null)
                {
                    ultimoRegistro = await this._context.Cuentas.Where(c => c.IdFletero == idFletero).OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }

                CuentaCorriente cuenta;
                if (ultimoRegistro == null)
                {
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda, pagado, 0);
                }
                else
                {
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda, pagado, ultimoRegistro.Saldo_Total);
                }

                await _context.Cuentas.AddAsync(cuenta);
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    return cuenta;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<CuentaCorrienteDTO> ObtenerCuentaMasRecientePorClienteIdAsync(int clienteId)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync()) 
                { 
                    return null;
                }

                var result = await _context.Cuentas.Where(r => r.IdCliente == clienteId).OrderByDescending(r => r.Fecha_factura).FirstOrDefaultAsync();
                if (result != null)
                {
                    CuentaCorrienteDTO cuenta = new CuentaCorrienteDTO((int)result.IdCliente, result.Fecha_factura, result.Nro_factura, result.Adeuda, result.Pagado, result.Saldo_Total, result.IdFletero, result.IdCliente);
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
                if (!await _context.Database.CanConnectAsync())
                {
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
                            .OrderByDescending(c => c.Id)
                            .Select(c => new CuentaCorrienteDTO(
                                c.Id,
                                c.Fecha_factura,
                                c.Nro_factura,
                                c.Adeuda,
                                c.Pagado,
                                c.Saldo_Total,
                                c.IdFletero,
                                c.IdCliente
                            ))
                            .ToListAsync();
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
                            .OrderByDescending(c => c.Id)
                            .Select(c => new CuentaCorrienteDTO(
                                c.Id,
                                c.Fecha_factura,
                                c.Nro_factura,
                                c.Adeuda,
                                c.Pagado,
                                c.Saldo_Total,
                                c.IdFletero,
                                c.IdCliente
                            ))
                            .ToListAsync();
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

                var cuentasHijas = await _context.Cuentas.Where(c => c.Id > id && c.IdCliente == cuenta.IdCliente && c.IdFletero == cuenta.IdFletero).ToListAsync();
                Console.WriteLine("sobrevivimos al método de obtener cuentas hijas");
                if(cuentasHijas != null)
                {
                    foreach(CuentaCorriente c in cuentasHijas)
                    {
                        Console.WriteLine("hola entré al foreach");
                        _context.Cuentas.Remove(c);

                        await _context.SaveChangesAsync();
                    }
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

        internal async Task<CuentaCorrienteDTO?> ActualizarAsync(int id, DateOnly? fecha, int? nroFactura, float? adeuda, float? importe, int? idCliente, int? idFletero)
        {
            try
            {
                CuentaCorriente? cuenta = await this._context.Cuentas.FindAsync(id);
                if (cuenta == null)
                {
                    return null;
                }
                if (fecha != null)
                {
                    cuenta.Fecha_factura = (DateOnly)fecha;
                }
                if (nroFactura != null)
                {
                    cuenta.Nro_factura = (int)nroFactura;
                }
                if (adeuda != null)
                {
                    cuenta.Adeuda = (float)adeuda;
                }
                if (importe != null)
                {
                    cuenta.Pagado = (float)importe;
                }
                if(idCliente != null)
                {
                    cuenta.IdCliente = idCliente;
                }
                if(idFletero != null)
                {
                    cuenta.IdFletero = idFletero;
                }
                if (importe != null || adeuda != null)
                {
                    Console.WriteLine("hola if de importe o adeuda???");
                    if (cuenta.IdCliente != null)
                    {
                        Console.WriteLine("hola if de idCliente != null");
                        CuentaCorriente? anteUltimoRegistro = await this._context.Cuentas
                                      .Where(c => c.IdCliente == cuenta.IdCliente && c.Id < cuenta.Id)
                                      .OrderByDescending(c => c.Id)
                                      .FirstOrDefaultAsync();
                        if (anteUltimoRegistro != null)
                        {
                            Console.WriteLine("anteultimo registro: " + anteUltimoRegistro.ToString());
                            Console.WriteLine(anteUltimoRegistro.Saldo_Total);
                            Console.WriteLine(adeuda);
                            cuenta.Saldo_Total = cuenta.Adeuda +anteUltimoRegistro.Saldo_Total - cuenta.Pagado;
                        }
                        else
                        {
                            cuenta.Saldo_Total = cuenta.Adeuda - cuenta.Pagado;
                        }
                    }
                    else if (cuenta.IdFletero != null)
                    {
                        CuentaCorriente? anteUltimoRegistro = await this._context.Cuentas
                                      .Where(c => c.IdFletero == cuenta.IdFletero && c.Id < cuenta.Id)
                                      .OrderByDescending(c => c.Id)
                                      .FirstOrDefaultAsync();
                        if (anteUltimoRegistro != null)
                        {
                            Console.WriteLine(anteUltimoRegistro.Saldo_Total);
                            Console.WriteLine(adeuda);
                            cuenta.Saldo_Total = cuenta.Adeuda + anteUltimoRegistro.Saldo_Total - cuenta.Pagado;
                        }
                        else
                        {
                            cuenta.Saldo_Total = cuenta.Adeuda - cuenta.Pagado;
                        }
                    }
                }
                int registrosAfectados = await _context.SaveChangesAsync();
                if (registrosAfectados > 0)
                {
                    Console.WriteLine("hola if de registros afectados?");


                    if(adeuda != null || importe != null)
                    {
                        //apartado para ver si la cuenta modificada tiene cuentas hijas
                        var cuentasHijas = await _context.Cuentas
                                               .Where(c => c.Id > id && c.IdCliente == cuenta.IdCliente && c.IdFletero == cuenta.IdFletero)
                                               .OrderBy(c => c.Id)
                                               .ToListAsync();
                        if (cuentasHijas != null)
                        {
                            CuentaCorriente padre = cuenta;
                            Console.WriteLine(padre.Saldo_Total);
                            foreach (CuentaCorriente c in cuentasHijas)
                            {
                                bool success = await this.ModificarHijo(c, padre.Saldo_Total);
                                if (!success)
                                {
                                    Console.WriteLine("fuck no fue success");
                                    return null;
                                }
                                padre = c;
                            }
                        }
                    }


                    return new CuentaCorrienteDTO(cuenta.Id, cuenta.Fecha_factura, cuenta.Nro_factura, cuenta.Adeuda, cuenta.Pagado, cuenta.Saldo_Total, cuenta.IdCliente, cuenta.IdFletero);
                }
                Console.WriteLine("fuck no entré al if de registros afectados");
                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }

        private async Task<bool> ModificarHijo(CuentaCorriente c, float saldo_Total)
        {
            try
            {
                c.Saldo_Total = c.Adeuda + saldo_Total - c.Pagado;
                int success = await _context.SaveChangesAsync();
                return success > 0;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return false;
            }
        }

        internal async Task<CuentaCorriente> ObtenerPorId(int id)
        {
            try
            {
                CuentaCorriente cuenta = await this._context.Cuentas.FindAsync(id);
                return cuenta;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
    }
}
