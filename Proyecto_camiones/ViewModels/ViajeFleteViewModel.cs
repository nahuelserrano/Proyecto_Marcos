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

        public async Task<Result<int>> InsertarViajeFlete(string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            if (this.testearConexion().Result)
            {
                nombre_cliente = nombre_cliente.ToUpper();
                nombre_fletero = nombre_fletero.ToUpper();
                return await this.fleteService.InsertarViajeFlete(origen, destino, remito, carga, km, kg, tarifa, factura, nombre_cliente, nombre_fletero, nombre_chofer, comision, fecha_salida);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la db");
            
        }

        public async Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFletero(string fletero)
        {
            if (this.testearConexion().Result)
            {
                fletero = fletero.ToUpper();
                return await this.fleteService.ObtenerViajesDeUnFletero(fletero);
            }
            return Result<List<ViajeFleteDTO>>.Failure("No se pudo acceder a la base de datos");
        }
    }
}
