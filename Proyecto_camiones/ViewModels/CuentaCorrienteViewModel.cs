using MySqlX.XDevAPI.Common;
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
            var dbContext = General.obtenerInstancia();
            var cr = new CuentaCorrienteRepository(dbContext);
            this.cs = new CuentaCorrienteService(cr, new ClienteRepository(dbContext));
        }

        public async Task<bool> testearConexion()
        {
            return await this.cs.ProbarConexionAsync();
        }

        public async Task<Result<int>> Insertar(string? cliente, string? fletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            if (this.testearConexion().Result)
            {
                int id = await cs.Insertar(cliente, fletero, fecha, nro, adeuda, pagado);
                if (id > -1) return Result<int>.Success(id);
                return Result<int>.Failure("No se pudo crear el nuevo registro");
            }
            return Result<int>.Failure("no se pudo establecer la conexion");
        }

        public async Task<Result<CuentaCorriente>> ObtenerMasRecienteByCliente(int idCliente)
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

        public async Task<Result<List<CuentaCorriente>>> ObtenerTodas()
        {
            if (this.testearConexion().Result)
            {
                List<CuentaCorriente> cuentas = await this.cs.ObtenerTodas();
                if (cuentas != null)
                {
                    return Result<List<CuentaCorriente>>.Success(cuentas);
                }
                return Result<List<CuentaCorriente>>.Failure("No se pudieron obtener las cuentas");
            }
            return Result<List<CuentaCorriente>>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<List<CuentaCorriente>>> ObtenerCuentasByClienteId(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.cs.ObtenerCuentasByIdCliente(id);
            }
            return Result<List<CuentaCorriente>>.Failure("No se pudo establecer la conexion");
        }
    }
}
