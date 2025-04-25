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
        /*

<<<<<<< HEAD
        private PagoRepository _pagoRepository;
        public PagoService(PagoRepository pagoRepository)
        {
            this._pagoRepository = pagoRepository;
        }


        public async Task<bool> ProbarConexionAsync()
=======
        public Result<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
>>>>>>> 05e3f5d98b2f92eaa726809ab033b119be5101d9
        {
            bool result = await _pagoRepository.ProbarConexionAsync();
            return result;
        }
        public async Task<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {


            try
            {
<<<<<<< HEAD

                int idPago = await _pagoRepository.Insertar(id_chofer, id_viaje, monto_pagado);
                if (idPago > 0)
                {
                    return idPago;
                }
=======
                int idPago = PagoRepository.Insertar(id_chofer, id_viaje, pagado=false, monto_pagado);
                if (idPago > 0)
                {
                    return Result<int>.Success(idPago);
        }
>>>>>>> 05e3f5d98b2f92eaa726809ab033b119be5101d9
                else
                {
                    Console.WriteLine(idPago+"pago services");
                    return -2;
                }
            }
            catch
            {
                return -1;
            }

        }
<<<<<<< HEAD

        //public async Task<bool> MarcarPagos(int id_chofer, DateOnly desde, DateOnly hasta, int id_Sueldo) {
        //    try
        //    {
               
=======
                
        public Result<bool> marcarPagos(int id_chofer,DateOnly desde,DateOnly hasta,int id_Sueldo) {
            try
            {
                List<Pago> pagos = PagoRepository.ObtenerPagos(id_chofer,desde,hasta);
                foreach (var pago in pagos)
                {                   
                      this.ActualizarAsync(pago.Id,true,id_Sueldo);
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al marcar los pagos: {ex.Message}");
            }
>>>>>>> 05e3f5d98b2f92eaa726809ab033b119be5101d9

              
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        Console.WriteLine($"Error al marcar los pagos: {ex.Message}");
        //    }

<<<<<<< HEAD
        //}


        public async Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {

            List<Pago> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);
=======
        //public async Task<Result<float>> ObtenerPorFiltroAsync(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        //{
             
        //    Task<List<Pago>> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);
>>>>>>> 05e3f5d98b2f92eaa726809ab033b119be5101d9

        public Result<float> ObtenerPorFiltroAsync(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {
            List<Pago> pagos = PagoRepository.ObtenerPagos(id_chofer, calcularDesde, calcularHasta);
            
            float totalPagar = 0;
<<<<<<< HEAD
            foreach (var pago in pagos) {
                Console.WriteLine(pago.Monto_Pagado);
                totalPagar += pago.Monto_Pagado;
            }
            return totalPagar;
        }


       
    
=======
            foreach (var pago in pagos) { 
           
                 totalPagar+=pago.Monto_Pagado;
            }
           return Result<float>.Success(totalPagar);
        }
        
        //public Result<bool> ActualizarAsync(int id,bool pagado,int id_sueldo) 
        //{
        //    PagoRepository.MarcarComoPagado(id,pagado,id_sueldo);
        //}
    
        public Result<bool> ActualizarAsync(int id,bool pagado,int id_sueldo) 
        {
            PagoRepository.MarcarComoPagado(id,pagado,id_sueldo);
        } */
>>>>>>> 05e3f5d98b2f92eaa726809ab033b119be5101d9
    }
}

