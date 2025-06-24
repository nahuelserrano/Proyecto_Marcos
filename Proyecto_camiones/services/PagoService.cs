using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Models;
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

        public async Task<Result<Pago>> ObtenerPorIdViajeAsync(int idViaje)
        {
            try
            {
                Pago? pago = await _pagoRepository.ObtenerPorIdViajeAsync(idViaje);

                if (pago == null)
                    return Result<Pago>.Failure($"No se encontró un pago para el viaje con ID: {idViaje}");

                return Result<Pago>.Success(pago);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los pagos: {ex.Message}");
                return Result<Pago>.Failure($"Error al obtener los pagos: {ex.Message}");
            }
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
                return -2;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<Result<bool>> ActualizarAsync(int id_chofer, int id_viaje, float? monto_pagado)
        {
            if (id_chofer <= 0 || id_viaje <= 0)
                return Result<bool>.Failure(MensajeError.IdInvalido(id_chofer));

            // Permitir monto_pagado null o verificar que sea mayor a 0 si tiene valor
            if (monto_pagado.HasValue && monto_pagado <= 0)
                return Result<bool>.Failure(MensajeError.valorInvalido(nameof(monto_pagado)));

            try
            {
                var pago = await _pagoRepository.ObtenerPorIdViajeAsync(id_viaje);

                if (pago == null)
                    return Result<bool>.Failure($"No se encontró un pago para el viaje con ID: {id_viaje}");

                if (pago.Pagado)
                    return Result<bool>.Failure("No se pudo actualizar el pago ya que el mismo ya se encuentra pagado");

                // CORREGIR EL ERROR DE SINTAXIS
                bool actualizado = await _pagoRepository.ActualizarAsync(id_chofer, id_viaje, monto_pagado);

                if (actualizado)
                {
                    return Result<bool>.Success(actualizado);
                }
                return Result<bool>.Failure(MensajeError.ErrorEliminacion(nameof(Pago)));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error al actualizar el pago");
            }
        }

        public async Task<Result<bool>> ModificarEstado(int id_chofer, DateOnly desde, DateOnly hasta, int? id_Sueldo,bool pagado = true) {
            try
            {
                bool actualizado=await _pagoRepository.modificarEstado(id_chofer, desde, hasta, id_Sueldo,pagado);
                if (actualizado) {
                    return Result<bool>.Success(actualizado);
                }
                return Result<bool>.Failure("No se pudo marcar los pagos");

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("error al marcar como pagados los pagos correspondientes al sueldo");
            }
        }


        public async Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta)
        {
            List<Pago> pagos = await _pagoRepository.ObtenerPagosAsync(id_chofer, calcularDesde, calcularHasta);

            float totalPagar = 0;

            foreach (var pago in pagos)
            {
                totalPagar += pago.Monto_Pagado;
            }
            return totalPagar;
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.IdInvalido(id));
            try
            {
                bool eliminado = await _pagoRepository.EliminarAsync(id);
                if (eliminado)
                {
                    return Result<bool>.Success(eliminado);
                }
                return Result<bool>.Failure(MensajeError.ErrorEliminacion(nameof(Pago)));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar el pago: {ex.Message}");
            }
        }
    }
}