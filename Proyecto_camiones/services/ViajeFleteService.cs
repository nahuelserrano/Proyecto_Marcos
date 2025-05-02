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
    public class ViajeFleteService
    {
        private ViajeFleteRepository ViajeFleteRepository;
        private ClienteRepository clienteRepository;
        private FleteRepository fleteRepository;

        public ViajeFleteService(ViajeFleteRepository fleteRepository, ClienteRepository cs, FleteRepository fr)
        {
            this.ViajeFleteRepository = fleteRepository ?? throw new ArgumentNullException(nameof(fleteRepository));
            this.clienteRepository = cs;
            this.fleteRepository = fr;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this.fleteRepository.ProbarConexionAsync();
            return result;
        }

        internal async Task<Result<int>> InsertarAsync(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            Cliente cliente = await this.clienteRepository.ObtenerPorNombre(nombre_cliente);
            Flete fletero = await this.fleteRepository.ObtenerPorNombreAsync(nombre_fletero);
            if(cliente != null && fletero != null) 
            {                                                                                                              
               int idViaje = await this.ViajeFleteRepository.InsertarAsync(origen, destino, remito, carga, km, kg, tarifa, factura, cliente.Id, fletero.Id, nombre_chofer, comision, fecha_salida);
                if (idViaje > 0)
                {
                    return Result<int>.Success(idViaje);
                }
                return Result<int>.Failure("No se pudo insertar el viaje");
            }
            return Result<int>.Failure("No se puede insertar el viaje, el cliente o el fletero con ese nombre no existe");
        }

        internal async Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFleteroAsync(string fletero)
        {
            Flete flete = await this.fleteRepository.ObtenerPorNombreAsync(fletero);
            if(flete != null)
            {
                List<ViajeFleteDTO> viajes = await this.ViajeFleteRepository.ObtenerViajesPorIdFleteroAsync(flete.Id);
                if (viajes != null)
                {
                   return Result<List<ViajeFleteDTO>>.Success(viajes);
                }
                return Result<List<ViajeFleteDTO>>.Failure("Hubo un error al obtener los viajes");
            }
            return Result<List<ViajeFleteDTO>>.Failure("No existe un fletero con ese nombre");
        }

    }
}
