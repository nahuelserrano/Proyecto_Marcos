using System;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.DTOs;
using NPOI.POIFS.Properties;
using System.Collections.Generic;

namespace Proyecto_camiones.Presentacion.Services
{
    class ChequeService
    {
        private ChequeRepository _chequeRepository;

        public ChequeService(ChequeRepository chequeR)
        {
            this._chequeRepository = chequeR ?? throw new ArgumentNullException(nameof(chequeR));
        }

        public async Task<Result<ChequeDTO>> ObtenerPorId(int id)
        {
            if (id < 0)
            {
                return Result<ChequeDTO>.Failure(MensajeError.idInvalido(id));
            }

            ChequeDTO cheque = await this._chequeRepository.ObtenerPorId(id);

            if (cheque == null)
                return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(cheque)));

            return Result<ChequeDTO>.Success(cheque);
        }

        internal async Task<Result<bool>> Eliminar(int id)
        {
            if (id <= 0) return Result<bool>.Failure(MensajeError.idInvalido(id));

            ChequeDTO cheque = await this._chequeRepository.ObtenerPorId(id);

            if (cheque == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(cheque)));

            await _chequeRepository.Eliminar(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> Crear(int id_Cliente, DateOnly FechaIngresoCheque, string NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
        {
            ValidadorCheque validador = new ValidadorCheque(id_Cliente, FechaIngresoCheque, NumeroCheque, Monto, Banco, FechaCobro);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);


            try
            {
                // Intentar insertar en la base de datos
                int resultado = await _chequeRepository.Insertar(id_Cliente, FechaIngresoCheque, NumeroCheque, Monto, Banco, FechaCobro);
                if(resultado< 0)
                    return Result<int>.Failure("El cheque no pudo ser insertado");
              
                return Result<int>.Success(resultado);


            }
            catch (Exception)
            {

                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }


        public async Task<Result<ChequeDTO>> Actualizar(int id, int? id_cliente = null, DateOnly? FechaIngresoCheque = null, string? NumeroCheque = null, float? Monto = null, string? Banco = null, DateOnly? FechaCobro = null)
        {
            if (id <= 0)
                return Result<ChequeDTO>.Failure(MensajeError.idInvalido(id));

            var chequeExistente = await _chequeRepository.ObtenerPorId(id);

            if (chequeExistente == null)
                return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(chequeExistente)));


            if (id_cliente.HasValue)
                chequeExistente.Id_cliente = id_cliente.Value;

            if (FechaIngresoCheque.HasValue)
                chequeExistente.FechaIngresoCheque = FechaIngresoCheque.Value;

            if (NumeroCheque != null)
                chequeExistente.NumeroCheque = NumeroCheque;

            if (Monto.HasValue)
                chequeExistente.Monto = Monto.Value;

            if (!string.IsNullOrWhiteSpace(Banco))
                chequeExistente.Banco = Banco;

            if (FechaCobro.HasValue)
                chequeExistente.FechaCobro = FechaCobro.Value;

            if (chequeExistente.FechaIngresoCheque > chequeExistente.FechaCobro)
                return Result<ChequeDTO>.Failure(MensajeError.fechaInvalida(nameof(Cliente)));



            bool success = await _chequeRepository.Actualizar(id,chequeExistente.Id_cliente,chequeExistente.FechaIngresoCheque,chequeExistente.NumeroCheque,chequeExistente.Banco,chequeExistente.Monto,chequeExistente.FechaCobro);
            if (success)
            {
                ChequeDTO result = await _chequeRepository.ObtenerPorId(id);
                return Result<ChequeDTO>.Success(result);
            }
            return Result<ChequeDTO>.Failure("No se pudo realizar la actualización");
        }

        public async Task<List<ChequeDTO>?> ObtenerTodos()
            {
            List<ChequeDTO> cheque = await _chequeRepository.ObtenerTodos();
            if (cheque != null)
            {
                return cheque;
            }
            return new List<ChequeDTO>();
        }
    }
}   

