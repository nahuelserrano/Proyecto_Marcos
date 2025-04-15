using Proyecto_camiones.DTOs;
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
    public class ViajeFleteService
    {
        private ViajeFleteRepository fleteRepository;
        private ClienteRepository clienteRepository;

        public ViajeFleteService(ViajeFleteRepository fleteRepository, ClienteRepository cs)
        {
            this.fleteRepository = fleteRepository ?? throw new ArgumentNullException(nameof(fleteRepository));
            this.clienteRepository = cs;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this.fleteRepository.ProbarConexionAsync();
            return result;
        }

        internal async Task<Result<int>> InsertarViajeFlete(string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            Cliente cliente = await this.clienteRepository.ObtenerPorNombre(nombre_cliente);
            //HACER LA MISMA VALIDACION CON EL NOMBRE DEL FLETERO
            if(cliente != null) //Y EL FLETERO ES DISTINTO DE NULL
            {                                                                                                              //REEMPLAZAR CON FLETE.ID
               int idViaje = await this.fleteRepository.InsertarViajeFlete(origen, destino, remito, carga, km, kg, tarifa, factura, cliente.Id, 1, nombre_chofer, comision, fecha_salida);
                if (idViaje > 0)
                {
                    return Result<int>.Success(idViaje);
                }
                return Result<int>.Failure("No se pudo insertar el viaje");
            }
            return Result<int>.Failure("No se puede insertar el viaje, el cliente o el fletero con ese nombre no existe");
        }

        internal async Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFletero(string fletero)
        {
            throw new NotImplementedException();
        }
    }
}
