using Proyecto_camiones.Core.Services;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.ViewModels
{
    public class ViajeFleteViewModel
    {
        private readonly IViajeFleteService _viajeFleteService;

        public ViajeFleteViewModel(IViajeFleteService viajeFleteService)
        {
            _viajeFleteService = viajeFleteService ?? throw new ArgumentNullException(nameof(viajeFleteService));
        }


        public async Task<bool> TestearConexion()
        {
            return await this._viajeFleteService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarAsync(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string? nombre_chofer, float comision, DateOnly fecha_salida)
        {
            if (await this.TestearConexion())
            {
                nombre_cliente = nombre_cliente.ToUpper();
                nombre_fletero = nombre_fletero.ToUpper();
                return await this._viajeFleteService.InsertarAsync(origen, destino, remito, carga, km, kg, tarifa, factura, nombre_cliente, nombre_fletero, nombre_chofer, comision, fecha_salida);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la db");

        }

        public async Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFleteroAsync(string fletero)
        {
            if (await this.TestearConexion())
            {
                fletero = fletero.ToUpper();
                return await this._viajeFleteService.ObtenerViajesDeUnFleteroAsync(fletero);
            }
            return Result<List<ViajeFleteDTO>>.Failure("No se pudo acceder a la base de datos");
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (await this.TestearConexion())
            {
                return await this._viajeFleteService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo acceder a la base de datos");
        }

        internal async Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnClienteAsync(int id)
        {
            if (await this.TestearConexion())
            {
                return await this._viajeFleteService.ObtenerViajesDeUnClienteAsync(id);
            }
            return Result<List<ViajeMixtoDTO>>.Failure("No se pudo acceder a la base de datos");
        }

        internal async Task<Result<ViajeFlete>> ActualizarAsync(int id, string? origen, string? destino, float? remito, string? carga, float? km, float? kg, float? tarifa, int? factura, string? cliente, string? nombre_chofer, float? comision, DateOnly? fecha_salida)
        {
            MessageBox.Show("actualizar");
            if (await this.TestearConexion())
            {
                MessageBox.Show("conexión testeada");
                return await this._viajeFleteService.ActualizarAsync(id, origen, destino, remito, carga, km, kg, tarifa, factura, cliente, nombre_chofer, comision, fecha_salida);
            }
            return Result<ViajeFlete>.Failure("No se pudo acceder a la base de datos");
        }
    }
}
