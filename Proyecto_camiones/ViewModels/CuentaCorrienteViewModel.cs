using MySqlX.XDevAPI.Common;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class CuentaCorrienteViewModel
    {
        public CuentaCorrienteService cs;
        public CuentaCorrienteViewModel()
        {
            var cr = new CuentaCorrienteRepository(General.obtenerInstancia());
            this.cs = new CuentaCorrienteService(cr, new ClienteRepository(General.obtenerInstancia()));
        }

        public async Task<bool> testearConexion()
        {
            return await this.cs.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarAsync(string? cliente, string? fletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            if (await this.testearConexion())
            {
                int id = await cs.Insertar(cliente, fletero, fecha, nro, adeuda, pagado);
                if (id > -1) return Result<int>.Success(id);
                return Result<int>.Failure("No se pudo crear el nuevo registro");
            }
            return Result<int>.Failure("no se pudo establecer la conexion");
        }

        public async Task<Result<CuentaCorriente>> ObtenerCuentaMasRecienteByClienteAsync(int idCliente)
        {
            if (this.testearConexion().Result)
            {
                CuentaCorriente c = await this.cs.ObtenerCuentaMasRecienteByClientId(idCliente);
                if(c != null)
                {
                    return Result<CuentaCorriente>.Success(c);
                }
                return Result<CuentaCorriente>.Failure("Hubo un problema al obtener la cuenta o no existe un cliente con ese id");
            }
            return Result<CuentaCorriente>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<List<CuentaCorriente>>> ObtenerTodosAsync()
        {
            if (this.testearConexion().Result)
            {
                List<CuentaCorriente> cuentas = await this.cs.ObtenerTodosAsync();
                if (cuentas != null)
                {
                    return Result<List<CuentaCorriente>>.Success(cuentas);
                }
                return Result<List<CuentaCorriente>>.Failure("No se pudieron obtener las cuentas");
            }
            return Result<List<CuentaCorriente>>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasByClienteAsync(string cliente)
        {
            if (await this.testearConexion())
            {
                return await this.cs.ObtenerCuentasByIdClienteAsync(cliente);
            }
            return Result<List<CuentaCorrienteDTO>>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasByFleteroAsync(string fletero)
        {
            if (this.testearConexion().Result)
            {
                return await this.cs.ObtenerCuentasDeUnFletero(fletero);
            }
            return Result<List<CuentaCorrienteDTO>>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.cs.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<CuentaCorrienteDTO>> ActualizarAsync(int id, DateOnly? fecha, int? nroFactura, float? adeuda, float? importe, int? idCliente, int? idFletero)
        {
            if (this.testearConexion().Result)
            {
                return await this.cs.ActualizarAsync(id, fecha, nroFactura, adeuda, importe, idCliente, idFletero);
            }
            return Result<CuentaCorrienteDTO>.Failure("No se pudo establecer la conexión");
        }
    }
}
