using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;


namespace Proyecto_camiones.Services
{
    public class PagoService
    {
        private PagoRepository _pagoRepository;
        public PagoService(PagoRepository pagoRepository)
        {
            this._pagoRepository = pagoRepository;
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await _pagoRepository.ProbarConexionAsync();
            return result;
        }
        public async Task<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {


            try
            {

                int idPago = await _pagoRepository.InsertarAsync(id_chofer, id_viaje, monto_pagado);
                if (idPago > 0)
                {
                    return idPago;
                }
                else
                {
                    Console.WriteLine(idPago + "pago services");
                    return -2;
                }
            }
            catch
            {
                return -1;
            }

        }

        public async Task<Result<bool>> MarcarPagos(int id_chofer, DateOnly desde, DateOnly hasta, int id_Sueldo) {
            try
            {
                bool actualizado=await _pagoRepository.ActualizarAsync(id_chofer, desde, hasta, id_Sueldo);
                if (actualizado) {
                    return Result<bool>.Success(actualizado);
                }
                return Result<bool>.Failure("No se pudo marcar los pagos");

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("error al marcar como pagados los pagos correspondientes al sueldo");
                Console.WriteLine($"Error al marcar los pagos: {ex.Message}");
            }

        }





        public async Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {

            List<Pago> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);

            float totalPagar = 0;
            foreach (var pago in pagos)
            {
                Console.WriteLine(pago.Monto_Pagado);
                totalPagar += pago.Monto_Pagado;
            }
            return totalPagar;






        }




    }
}