using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Utils;

namespace Proyecto_Marcos.Presentacion.Services
{
    class PagosService
    {
        private PagoRepository _pagoRepository;

        public PagosService(PagoRepository pagosR)
        {
            this._pagoRepository = pagosR ?? throw new ArgumentNullException(nameof(pagosR));
        }

        public async Task<Result<Pago>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<Pago>.Failure("El id no puede ser menor a 0");

            Pago Pago = await this._pagoRepository.getById(id);

            if (Pago == null)
                return Result<Pago>.Failure("El cheque con el id " + id + " No existe");

            return Result<Pago>.Success(Pago);
        }

        internal async Task<Result<bool>> eliminarChequeAsync(int pagoId)
        {
            if (pagoId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

            Pago pago = await this._pagoRepository.getById(pagoId);

            if (pago == null) return Result<bool>.Failure("El cheque con el id " + pagoId + " No existe");

            this._pagoRepository.eliminarcheque(pagoId);

            return Result<bool>.Success(true);

        }

        public async Task<Result<int>> CrearPagoAsync(Pago pago)
        {
            if (pago.Monto == null || pago.Pagado == null) return Result<int>.Failure("¡datos incompletos!");


            if (pago.Monto < 0) return Result<int>.Failure("La fecha de ingreso del cheque no puede ser posterior a la fecha de cobro.");

            try
            {
                // Intentar insertar en la base de datos
                int idPago = await _pagoRepository.insertarchequeAsync(pago);
                return Result<int>.Success(idPago);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }

        public async Task<Result<int>> EditarPagoAsync(int id, float? monto = null, bool? pagado = null)
        {
            if (id <= 0)
                return Result<int>.Failure("ID de pago inválido.");

            var pagarExistente = await _pagoRepository.ObtenerPorIdAsync(id);

            if (pagarExistente == null)
                return Result<int>.Failure("El pago no existe.");


            if (monto.HasValue)
                pagarExistente.Monto = monto.Value;

            if (pagado.HasValue)
                pagarExistente.Pagado = pagado.Value;

            try
            {

                await _pagoRepository.ActualizarAsync(pagarExistente);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al actualizar el pago con el id: " + id);
            }
        }

    }
}
