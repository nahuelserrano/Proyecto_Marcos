using MySqlX.XDevAPI.Common;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Services
{
    public class CuentaCorrienteService
    {
        private CuentaCorrienteRepository ccRepository;
        private ClienteRepository clienteRepository;
        private FleteRepository fleteRepository;

        public CuentaCorrienteService(CuentaCorrienteRepository cc)
        {
            this.ccRepository = cc ?? throw new ArgumentNullException(nameof(cc));
            this.clienteRepository = new ClienteRepository(General.obtenerInstancia());
            this.fleteRepository = new FleteRepository();
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this.ccRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<List<CuentaCorriente>> ObtenerTodosAsync()
        {
            List<CuentaCorriente> result = await ccRepository.ObtenerTodosAsync();
            return result;
        }

        public async Task<CuentaCorrienteDTO> ObtenerCuentaMasRecienteByClientId(int id)
        {
            Cliente c = await clienteRepository.ObtenerPorIdAsync(id);
            if(c == null)
            {
                return null;
            }
            CuentaCorrienteDTO result = await ccRepository.ObtenerCuentaMasRecientePorClienteIdAsync(id);
            return result;
        }

        public async Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasByIdClienteAsync(string cliente)
        {
            Cliente c = await clienteRepository.ObtenerPorNombreAsync(cliente.ToUpper());
            if (c == null)
            {
                return Result<List<CuentaCorrienteDTO>>.Failure("No existe un cliente con ese nombre");
            }
            int id = c.Id;
            List<CuentaCorrienteDTO> cuentas = await this.ccRepository.ObtenerCuentasPorIdClienteAsync(id);
            if(cuentas == null || cuentas.Count() == 0)
            {
                return Result<List<CuentaCorrienteDTO>>.Failure("No existen cuentas para ese cliente o hubo un fallo en la conexión");
            }
            return Result<List<CuentaCorrienteDTO>>.Success(cuentas);
        }

        public async Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasDeUnFletero(string fletero)
        {
            Flete f = await this.fleteRepository.ObtenerPorNombreAsync(fletero.ToUpper());
            if(f == null)
            {
                return Result<List<CuentaCorrienteDTO>>.Failure("No existe un fletero con ese nombre");
            }
            int id = f.Id;
            List<CuentaCorrienteDTO> cuentas = await this.ccRepository.ObtenerCuentasDeUnFleteroAsync(id);
            if(cuentas == null || cuentas.Count() == 0)
            {
                return Result<List<CuentaCorrienteDTO>>.Failure("No existen cuentas para ese fletero o hubo un fallo en la conexión");
            }
            return Result<List<CuentaCorrienteDTO>>.Success(cuentas);
        }

        public async Task<int> Insertar(string? cliente, string? fletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            if (cliente == null && fletero == null || cliente!= null && fletero != null) return -1;
            Cliente c;
            if(cliente != null)
            {
                c = await clienteRepository.ObtenerPorNombreAsync(cliente.ToUpper());
                if (c == null)
                {
                    return -1;
                }
                Console.WriteLine("corroborado que el cliente/fletero existe y no se salió");
                CuentaCorriente result = await ccRepository.InsertarAsync(c.Id, null, fecha, nro, adeuda, pagado);
                Console.WriteLine("superado 1");
                if (result != null)
                {
                    return result.Id;
                }
                return -1;
            }
            Flete flete;
            if(fletero != null)
            {
                flete = await this.fleteRepository.ObtenerPorNombreAsync(fletero.ToUpper());
                if (flete == null) return -1;
                CuentaCorriente result = await ccRepository.InsertarAsync(null, flete.Id, fecha, nro, adeuda, pagado);
                Console.WriteLine("superado 1");
                if (result != null)
                {
                    return result.Id;
                }
                return -1;
            }
            return -1;
        }

        internal async Task<Result<bool>> EliminarAsync(int id)
        {
            Cliente cliente = await this.clienteRepository.ObtenerPorIdAsync(id);
            if(cliente!= null)
            {
                bool response = await this.ccRepository.EliminarAsync(id);
                if (response)
                {
                    return Result<bool>.Success(response);
                }
                return Result<bool>.Failure("No se pudo eliminar");
            }
            return Result<bool>.Failure("No existe un cliente con ese id");
        }

        internal async Task<Result<CuentaCorrienteDTO>> ActualizarAsync(int id, DateOnly? fecha, int? nroFactura, float? adeuda, float? importe, string? cliente, string? fletero)
        {
            if(cliente != null && fletero != null || cliente == null && fletero == null)
            {
                return Result<CuentaCorrienteDTO>.Failure("No se puede actualizar la cuenta corriente ya que faltan datos del cliente o el fletero, o se quiso poner una cuenta corriente para 2 tipos de entidades no compatibles");
            }
            CuentaCorriente cuenta = await this.ccRepository.ObtenerPorId(id);
            if(cuenta == null)
            {
                return Result<CuentaCorrienteDTO>.Failure("Error al actualizar, no se encontró una cuenta con ese id");
            }

            int? idCliente = null;
            int? idFletero = null;
            if(cliente != null)
            {
                Cliente? c = await this.clienteRepository.ObtenerPorNombreAsync(cliente);
                if(c!= null)
                {
                    idCliente = c.Id;
                }
                else
                {
                    return Result<CuentaCorrienteDTO>.Failure("No se puede actualizar la cuenta corriente ya que no existe un cliente con el nombre ingresado");
                }
            } else if(fletero != null)
            {
                Flete? f = await this.fleteRepository.ObtenerPorNombreAsync(fletero);
                if(f!= null)
                {
                    idFletero = f.Id;
                }
                else
                {
                    return Result<CuentaCorrienteDTO>.Failure("No se puede actualizar la cuenta corriente ya que no existe un fletero con el nombre ingresado");
                }
            }

                CuentaCorrienteDTO actualizada = await this.ccRepository.ActualizarAsync(id, fecha, nroFactura, adeuda, importe, idCliente, idFletero);
            if(actualizada != null)
            {
                return Result<CuentaCorrienteDTO>.Success(actualizada);
            }
            return Result<CuentaCorrienteDTO>.Failure("Error interno al actualizar");
        }
    }
}
