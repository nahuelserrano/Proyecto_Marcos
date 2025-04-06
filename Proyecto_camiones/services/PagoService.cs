using System;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    class PagosService
    {
        private PagoRepository _pagoRepository;

        public PagosService(PagoRepository pagosR)
        {
            this._pagoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
        }

        public async Task<Result<Pago>> ObtenerPorId(int id)
        {
            if (id <= 0)
                return Result<Pago>.Failure(MensajeError.idInvalido(id));

            Pago Pago = await this._pagoRepository.ObtenerPorId(id);

            if (Pago == null)
                return Result<Pago>.Failure(MensajeError.objetoNulo(nameof(Pago)));

            return Result<Pago>.Success(Pago);
        }

        internal async Task<Result<bool>> EliminarCheque(int pagoId)
        {
            if (pagoId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(pagoId));

            Pago pago = await this._pagoRepository.ObtenerPorId(pagoId);

            if (pago == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(pago)));

            // this._pagoRepository.EliminarCheque(pagoId); A implemenar en el repositorio

            return Result<bool>.Success(true);

        }

        public async Task<Result<int>> Crear(float monto, bool pagado)
        {

            ValidadorPago validador = new ValidadorPago( monto,pagado);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                // Intentar insertar en la base de datos
                int idPago = await _pagoRepository.Insertar(monto, pagado);
                return Result<int>.Success(idPago);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }

        public async Task<Result<int>> Actualizar(int id, float? monto = null, bool? pagado = null)
        {
            if (id <= 0)
                return Result<int>.Failure(MensajeError.idInvalido(id));

            var pagarExistente = await _pagoRepository.ObtenerPorId(id);

            if (pagarExistente == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(pagarExistente)));


            if (monto.HasValue)
                pagarExistente.Monto = monto.Value;

            if (pagado.HasValue)
                pagarExistente.Pagado = pagado.Value;

            try
            {

                await _pagoRepository.Actualizar(pagarExistente);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al actualizar el pago con el id: " + id);
            }
        }

    }
}
