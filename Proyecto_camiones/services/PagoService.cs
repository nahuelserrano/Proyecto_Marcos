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

                int idPago = await _pagoRepository.Insertar(id_chofer, id_viaje, monto_pagado);
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

        //public async Task<bool> MarcarPagos(int id_chofer, DateOnly desde, DateOnly hasta, int id_Sueldo) {
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        Console.WriteLine($"Error al marcar los pagos: {ex.Message}");
        //    }

        //}





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