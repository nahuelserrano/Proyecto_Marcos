﻿using MySqlX.XDevAPI.Common;
using Proyecto_camiones.DTOs;
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
        private FleteRepository fleteRepository;

        public CuentaCorrienteService(CuentaCorrienteRepository cc, ClienteRepository cr)
        {
            this.ccRepository = cc ?? throw new ArgumentNullException(nameof(cc));
            this.clienteRepository = cr ?? throw new ArgumentNullException(nameof(cr));
            this.fleteRepository = new FleteRepository();
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

        public async Task<Result<List<CuentaClienteDTO>>> ObtenerCuentasByIdCliente(int id)
        {
            Cliente c = await clienteRepository.ObtenerPorId(id);
            if (c == null)
            {
                return Result<List<CuentaClienteDTO>>.Failure("No existe un cliente con ese Id");
            }
            List<CuentaClienteDTO> cuentas = await this.ccRepository.ObtenerCuentasByIdCliente(id);
            if(cuentas == null || cuentas.Count() == 0)
            {
                return Result<List<CuentaClienteDTO>>.Failure("No existen cuentas para ese cliente o hubo un fallo en la conexión");
            }
            return Result<List<CuentaClienteDTO>>.Success(cuentas);
        }

        public async Task<int> Insertar(string? cliente, string? fletero, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            if (cliente == null && fletero == null) return -1;
            Cliente c;
            if(cliente != null)
            {
                c = await clienteRepository.ObtenerPorNombre(cliente.ToUpper());
                if (c == null)
                {
                    return -1;
                }
                Console.WriteLine("corroborado que el cliente/fletero existe y no se salió");
                CuentaCorriente result = await ccRepository.InsertarCuentaCorriente(c.Id, null, fecha, nro, adeuda, pagado);
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
                flete = await this.fleteRepository.ObtenerPorNombre(fletero.ToUpper());
                if (flete == null) return -1;
                CuentaCorriente result = await ccRepository.InsertarCuentaCorriente(null, flete.Id, fecha, nro, adeuda, pagado);
                Console.WriteLine("superado 1");
                if (result != null)
                {
                    return result.Id;
                }
                return -1;
            }
            return -1;
            
        }
    }
}
