using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Utils;

namespace Proyecto_Marcos.Presentacion.Services
{
    class ChequeService
    {
        private ChequeRepository _chequeRepository;

        public ChequeService(ChequeRepository chequeR)
        {
            this._chequeRepository = chequeR ?? throw new ArgumentNullException(nameof(chequeR));
        }

        public async Task<Result<Cheque>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<Cheque>.Failure("El id no puede ser menor a 0");

            Cheque cheque = await this._chequeRepository.getById(id);

            if (cheque == null)
                return Result<Cheque>.Failure("El cheque con el id " + id + " No existe");

            return Result<Cheque>.Success(cheque);
        }

        internal async Task<Result<bool>> eliminarChequeAsync(int chequeId)
        {
            if (chequeId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

            Cheque cheque = await this._chequeRepository.getById(chequeId);

            if (cheque == null) return Result<bool>.Failure("El cheque con el id " + chequeId + " No existe");

            this._chequeRepository.eliminarcheque(chequeId);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> CrearChequeAsync(Cheque cheque)
        {
            if (cheque.Cliente_Dueño_Cheque == null || cheque.FechaIngresoCheque == null || cheque.NumeroCheque == null || cheque.Monto == null || cheque.Banco == null || cheque.FechaCobro == null) return Result<int>.Failure("¡datos incompletos!");

            if (cheque.FechaIngresoCheque.Date > cheque.FechaCobro.Date)  return Result<int>.Failure("La fecha de ingreso del cheque no puede ser posterior a la fecha de cobro.");

            try
            {
                // Intentar insertar en la base de datos
                int idcheque = await _chequeRepository.insertarchequeAsync(cheque);
                return Result<int>.Success(idcheque);
            }
            catch (Exception ex)
            {
                
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }

        public async Task<Result<int>> EditarChequeAsync(int id, Cliente cliente = null, DateTime? FechaIngresoCheque = null, int? NumeroCheque = null, float? Monto = null, string Banco = null, DateTime? FechaCobro = null)
        {
            if (id <= 0)
                return Result<int>.Failure("ID de cheque inválido.");

            var chequeExistente = await _chequeRepository.ObtenerPorIdAsync(id);

            if (chequeExistente == null)
                return Result<int>.Failure("El cheque no existe.");

          
            if (cliente != null)
                chequeExistente.Cliente_Dueño_Cheque = cliente;

            if (FechaIngresoCheque.HasValue)
                chequeExistente.FechaIngresoCheque = FechaIngresoCheque.Value;

            if (NumeroCheque.HasValue)
                chequeExistente.NumeroCheque = NumeroCheque.Value;

            if (Monto.HasValue)
                chequeExistente.Monto = Monto.Value;

            if (!string.IsNullOrWhiteSpace(Banco))
                chequeExistente.Banco = Banco;

            if (FechaCobro.HasValue)
                chequeExistente.FechaCobro = FechaCobro.Value;

            if (chequeExistente.FechaIngresoCheque > chequeExistente.FechaCobro)
                return Result<int>.Failure("La fecha de ingreso del cheque no puede ser posterior a la fecha de cobro.");

            try
            {
                await _chequeRepository.ActualizarAsync(chequeExistente);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al actualizar el cheque: {ex.Message}");
            }
        }
    }
}
