using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.ViewModels
{
    public class ViajeFleteViewModel
    {
        private ViajeFleteService fleteService;

        public ViajeFleteViewModel()
        {
            var repo = new ViajeFleteRepository();
            this.fleteService = new ViajeFleteService(repo, new ClienteRepository(General.obtenerInstancia()), new FleteRepository());
        }


        public async Task<bool> testearConexion()
        {
            return await this.fleteService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarAsync(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            if (this.testearConexion().Result)
            {
                nombre_cliente = nombre_cliente.ToUpper();
                nombre_fletero = nombre_fletero.ToUpper();
                return await this.fleteService.InsertarAsync(origen, destino, remito, carga, km, kg, tarifa, factura, nombre_cliente, nombre_fletero, nombre_chofer, comision, fecha_salida);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la db");

        }

        public async Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFleteroAsync(string fletero)
        {
            if (await this.testearConexion())
            {
                fletero = fletero.ToUpper();
                return await this.fleteService.ObtenerViajesDeUnFleteroAsync(fletero);
            }
            return Result<List<ViajeFleteDTO>>.Failure("No se pudo acceder a la base de datos");
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.fleteService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo acceder a la base de datos");
        }

        internal async Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnClienteAsync(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.fleteService.ObtenerViajesDeUnClienteAsync(id);
            }
            return Result<List<ViajeMixtoDTO>>.Failure("No se pudo acceder a la base de datos");
        }

        internal async Task<Result<ViajeFlete>> ActualizarAsync(int id, string? origen, string? destino, float? remito, string? carga, float? km, float? kg, float? tarifa, int? factura, string? cliente, string? nombre_chofer, float? comision, DateOnly? fecha_salida)
        {
            if (this.testearConexion().Result)
            {
                return await this.fleteService.ActualizarAsync(id, origen, destino, remito, carga, km, kg, tarifa, factura, cliente, nombre_chofer, comision, fecha_salida);
            }
            return Result<ViajeFlete>.Failure("No se pudo acceder a la base de datos");
        }
    }
}
