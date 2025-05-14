using Microsoft.EntityFrameworkCore;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion;
using Proyecto_camiones.Presentacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    MessageBox.Show("No se puede conectar a la base de datos");
                    Console.WriteLine("No se puede conectar a la base de datos");
                    return null;
                }
                CuentaCorriente ultimoRegistro;
                if (idCliente != null)
                {
                    MessageBox.Show("id cliente: " + idCliente);
                    ultimoRegistro = await this._context.Cuentas.Where(c => c.IdCliente == idCliente).OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                    MessageBox.Show("ultimo registro: " + ultimoRegistro);
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
                    MessageBox.Show(ultimoRegistro + "es null");
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda, pagado, 0);
                }
                else
                {
                    Console.WriteLine(ultimoRegistro.Saldo_Total);
                    Console.WriteLine(adeuda + ultimoRegistro.Saldo_Total - pagado);
                    cuenta = new CuentaCorriente(idCliente, idFletero, fecha, nro, adeuda, pagado, ultimoRegistro.Saldo_Total);
                }

                await _context.Cuentas.AddAsync(cuenta);
                int registrosAfectados = await _context.SaveChangesAsync();

                if (registrosAfectados > 0)
                {
                    MessageBox.Show("hay registros afectados: " + registrosAfectados);
                    return cuenta;
                }
                MessageBox.Show("no hay registros afectados: " + registrosAfectados);
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

        public async Task<CuentaCorrienteDTO> ObtenerCuentaMasRecientePorClienteIdAsync(int clienteId)
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

        internal async Task<CuentaCorrienteDTO> ActualizarAsync(int id, DateOnly? fecha, int? nroFactura, float? adeuda, float? importe, int? idCliente, int? idFletero)
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
