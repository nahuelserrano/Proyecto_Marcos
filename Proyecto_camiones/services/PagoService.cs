using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;


namespace Proyecto_camiones.Services
{
    class PagoServices
    {

        private PagoRepository _pagoRepository;
        public PagoServices(PagoRepository pagoRepository)
        {
            this._pagoRepository = pagoRepository;
        }
        public Result<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado)
        {
            ValidadorPago validador = new ValidadorPago(id_chofer, id_viaje, monto_pagado);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();
            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                
                int idPago = PagoRepository.Insertar(id_chofer, id_viaje, monto_pagado);
                if (idPago > 0)
                {
                    return Result<int>.Success(idPago);
                }
                else
                {
                    return Result<int>.Failure("El pago no pudo ser insertado");
                }
            }
            catch
            {
                return Result<int>.Failure("Hubo un error al crear el pago");
            }

        }

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

        }
            

        public async Task<Result<float>> ObtenerPorFiltroAsync(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {
             
            Task<List<Pago>> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);

            float totalPagar = 0;
            foreach (var pago in pagos) { 
            
                 totalPagar+=pago.Monto_Pagado;
            }
           return Result<float>.Success(totalPagar);
        }
           
        
        public Result<bool> ActualizarAsync(int id,bool pagado,int id_sueldo) 
        {
            PagoRepository.MarcarComoPagado(id,pagado,id_sueldo);
        }
    
    }
}
