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
            var dbContext = General.obtenerInstancia();
            var repo = new ViajeFleteRepository(dbContext);
            this.fleteService = new ViajeFleteService(repo, new ClienteRepository(dbContext));
        }


        public async Task<bool> testearConexion()
        {
            return await this.fleteService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarViajeFlete(string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            return await this.fleteService.InsertarViajeFlete(origen, destino, remito, carga, km, kg, tarifa, factura, nombre_cliente, nombre_fletero, nombre_chofer, comision, fecha_salida);
        }
    }
}
