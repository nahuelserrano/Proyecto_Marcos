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
        private readonly ChequeRepository _chequeRepository;
        private readonly ClienteService _clienteService;

        public ChequeService(ChequeRepository chequeR, ClienteService clienteS)
        {
            this._chequeRepository = chequeR ?? throw new ArgumentNullException(nameof(chequeR));
            this._clienteService = clienteS ?? throw new ArgumentNullException(nameof(clienteS));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._chequeRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<ChequeDTO>> ObtenerPorIdAsync(String nroCheque)
        {
            if (String.IsNullOrEmpty(nroCheque))
                return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nroCheque));
            
            Console.WriteLine("ChequeService: ObtenerPorIdAsync");
            ChequeDTO? cheque = await this._chequeRepository.ObtenerPorNumeroChequeAsync(nroCheque);

            Console.WriteLine("3");
            if (cheque == null)
                return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(cheque)));

            return Result<ChequeDTO>.Success(cheque);
        }

        internal async Task<Result<bool>> EliminarAsync(String nroCheque)
        {
            try
            {
                if (nroCheque==null) return Result<bool>.Failure(MensajeError.objetoNulo(nroCheque));

                ChequeDTO? cheque = await this._chequeRepository.ObtenerPorNumeroChequeAsync(nroCheque);

                if (cheque == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(cheque)));

                await _chequeRepository.EliminarAsync(nroCheque);

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<int>> CrearAsync(int id_Cliente, DateOnly FechaIngresoCheque, String NumeroCheque, float Monto, string Banco, DateOnly FechaCobro)
        {
            ValidadorCheque validador = new ValidadorCheque(id_Cliente, FechaIngresoCheque, NumeroCheque, Monto, Banco, FechaCobro);
            //Result<bool> resultadoValidacion = validador.ValidarCompleto();

            //if (!resultadoValidacion.IsSuccess)
            //    return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                var clienteResult = await _clienteService.ObtenerPorIdAsync(id_Cliente);

                if (!clienteResult.IsSuccess)
                    return Result<int>.Failure(clienteResult.Error);

                // Intentar insertar en la base de datos
                int resultado = await _chequeRepository.InsertarAsync(id_Cliente, FechaIngresoCheque, NumeroCheque, Monto, Banco, FechaCobro);
                if (resultado < 0)
                    return Result<int>.Failure("El cheque no pudo ser insertado");

                return Result<int>.Success(resultado);

            }
            catch (Exception)
            {

                return Result<int>.Failure("Hubo un error al crear el cheque");
            }
        }


        //public async Task<Result<ChequeDTO>> ActualizarAsync(int id, int? id_cliente = null, DateOnly? FechaIngresoCheque = null, int? NumeroCheque = null, float? Monto = null, string? Banco = null, DateOnly? FechaCobro = null)
        //{
        //    if (id <= 0)
        //        return Result<ChequeDTO>.Failure(MensajeError.IdInvalido(id));

        //    var chequeExistente = await _chequeRepository.ObtenerPorIdAsync(id);

        //    if (chequeExistente == null)
        //        return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(chequeExistente)));


        //    if (id_cliente.HasValue)
        //        chequeExistente.Id_cliente = id_cliente.Value;

        //    if (FechaIngresoCheque.HasValue)
        //        chequeExistente.FechaIngresoCheque = FechaIngresoCheque.Value;

        //    if (NumeroCheque != null)
        //        chequeExistente.NumeroCheque = (int)NumeroCheque;

        //    if (Monto.HasValue)
        //        chequeExistente.Monto = Monto.Value;

        //    if (!string.IsNullOrWhiteSpace(Banco))
        //        chequeExistente.Banco = Banco;

        //    if (FechaCobro.HasValue)
        //        chequeExistente.FechaCobro = FechaCobro.Value;

        //    if (chequeExistente.FechaIngresoCheque > chequeExistente.FechaCobro)
        //        return Result<ChequeDTO>.Failure(MensajeError.fechaInvalida(nameof(Cliente)));



        //    bool success = await _chequeRepository.ActualizarAsync(id, chequeExistente.Id_cliente, chequeExistente.FechaIngresoCheque, chequeExistente.NumeroCheque, chequeExistente.Banco, chequeExistente.Monto, chequeExistente.FechaCobro);
        //    if (success)
        //    {
        //        ChequeDTO? result = await _chequeRepository.ObtenerPorIdAsync(id);
        //        return Result<ChequeDTO>.Success(result);
        //    }
        //    return Result<ChequeDTO>.Failure("No se pudo realizar la actualización");
        //}

        public async Task<List<ChequeDTO>?> ObtenerTodosAsync()
        {
            List<ChequeDTO>? cheques = await _chequeRepository.ObtenerTodosAsync();
            if (cheques != null)
            {
                return cheques;
            }
            return null;
        }
    }
}

