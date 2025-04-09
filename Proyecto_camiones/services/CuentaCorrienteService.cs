using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
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

        public CuentaCorrienteService(CuentaCorrienteRepository cc, ClienteRepository cr)
        {
            this.ccRepository = cc ?? throw new ArgumentNullException(nameof(cc));
            this.clienteRepository = cr ?? throw new ArgumentNullException(nameof(cr));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this.ccRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<List<CuentaCorriente>> ObtenerTodas()
        {
            List<CuentaCorriente> result = await ccRepository.ObtenerTodas();
            return result;
        }

        public async Task<CuentaCorriente> ObtenerCuentaMasRecienteByClientId(int id)
        {
            Cliente c = await clienteRepository.ObtenerPorId(id);
            if(c == null)
            {
                return null;
            }
            CuentaCorriente result = await ccRepository.ObtenerCuentaMasRecienteByClienteId(id);
            return result;
        }

        public async Task<Result<List<CuentaCorriente>>> ObtenerCuentasByIdCliente(int id)
        {
            Cliente c = await clienteRepository.ObtenerPorId(id);
            if (c == null)
            {
                return Result<List<CuentaCorriente>>.Failure("No existe un cliente con ese Id");
            }
            List<CuentaCorriente> cuentas = await this.ccRepository.ObtenerCuentasByIdCliente(id);
            if(cuentas == null || cuentas.Count() == 0)
            {
                return Result<List<CuentaCorriente>>.Failure("No existen cuentas para ese cliente o hubo un fallo en la conexión");
            }
            return Result<List<CuentaCorriente>>.Success(cuentas);
        }

        public async Task<int> Insertar(int idCliente, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            Cliente c = await clienteRepository.ObtenerPorId(idCliente);
            if (c == null)
            {
                return -1;
            }
            Console.WriteLine("corroborado que el cliente existe y no se salió");
            CuentaCorriente result = await ccRepository.InsertarCuentaCorriente(idCliente, fecha, nro, adeuda, pagado);
            Console.WriteLine("superado 1");
            if(result != null)
            {
                return result.Id;
            }
            return -1;
        }
    }
}
