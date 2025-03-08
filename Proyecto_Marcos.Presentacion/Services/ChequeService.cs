using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Utils;

namespace Proyecto_Marcos.Presentacion.Services
{
    class ChequeService
    {
        private ChequeRepository _chequeRepository;
        private Validador _validador;
        public ChequeService(ChequeRepository chequeR)
        {
            this._chequeRepository = chequeR ?? throw new ArgumentNullException(nameof(chequeR));
        }

        public async Task<Result<Cheque>> ObtenerPorId(int id)
        {
           this.ValidarId(id);

            Cheque cheque = await this._chequeRepository.ObtenerPorId(id);

            if (cheque == null)
                return Result<Cheque>.Failure("El cheque con el id " + id + " No existe");

            return Result<Cheque>.Success(cheque);
        }

        internal async Task<Result<bool>> Eliminar(int chequeId)
        {
            if (chequeId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

            Cheque cheque = await this._chequeRepository.ObtenerPorId(chequeId);

            if (cheque == null) return Result<bool>.Failure("El cheque con el id " + chequeId + " No existe");

            this._chequeRepository.Eliminar(chequeId);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> Crear(Cheque cheque)
        {
            if (cheque.Cliente_Dueño_Cheque == null || cheque.FechaIngresoCheque == null || cheque.NumeroCheque == null || cheque.Monto == null || cheque.Banco == null || cheque.FechaCobro == null) return Result<int>.Failure("¡datos incompletos!");

            if (cheque.FechaIngresoCheque.Date > cheque.FechaCobro.Date)  return Result<int>.Failure("La fecha de ingreso del cheque no puede ser posterior a la fecha de cobro.");

            try
            {
                // Intentar insertar en la base de datos
                int idcheque = await _chequeRepository.Insertar(cheque);
                return Result<int>.Success(idcheque);
            }
            catch (Exception ex)
            {
                
                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }

        public async Task<Result<int>> Actualizar(int id, Cliente cliente = null, DateTime? FechaIngresoCheque = null, int? NumeroCheque = null, float? Monto = null, string Banco = null, DateTime? FechaCobro = null)
        {
            if (id <= 0)
                return Result<int>.Failure(MensajeError.idInvalido(id));

            var chequeExistente = await _chequeRepository.ObtenerPorId(id);

            if (chequeExistente == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(chequeExistente)));

          
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
                return Result<int>.Failure(MensajeError.fechaInvalida(nameof(cliente)));

            try
            {
                await _chequeRepository.Actualizar(chequeExistente);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al actualizar el cheque: {ex.Message}");
            }
        }
    }
}
