﻿using System;
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

        public async Task<Result<int>> CrearAsync(
            DateOnly fechaIngreso,
            int numeroCheque,
            float monto,
            string banco,
            DateOnly fechaCobro,
            string nombre = "",
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null)
        {
            // Validación básica
            if (numeroCheque <= 0)
                return Result<int>.Failure("El número de cheque debe ser mayor a cero");

            if (monto <= 0)
                return Result<int>.Failure("El monto debe ser mayor a cero");

            if (string.IsNullOrWhiteSpace(banco))
                return Result<int>.Failure(MensajeError.atributoRequerido("Banco"));

            try
            {
                // Intentar insertar en la base de datos
                int resultado = await _chequeRepository.InsertarAsync(
                    fechaIngreso,
                    numeroCheque,
                    monto,
                    banco,
                    fechaCobro,
                    nombre,
                    numeroPersonalizado,
                    fechaVencimiento);

                if (resultado < 0)
                    return Result<int>.Failure("El cheque no pudo ser insertado");

                return Result<int>.Success(resultado);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al crear el cheque: {ex.Message}");
            }
        }
        public async Task<Result<ChequeDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<ChequeDTO>.Failure(MensajeError.IdInvalido(id));

            ChequeDTO? cheque = await this._chequeRepository.ObtenerPorIdAsync(id);

            if (cheque == null)
                return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(cheque)));

            return Result<ChequeDTO>.Success(cheque);
        }

        public async Task<Result<ChequeDTO>> ObtenerPorNumeroAsync(int nroCheque)
        {
            if (nroCheque <= 0)
                return Result<ChequeDTO>.Failure("El número de cheque debe ser mayor a cero");

            try
            {
                ChequeDTO? cheque = await _chequeRepository.ObtenerPorNumeroChequeAsync(nroCheque);

                if (cheque == null)
                    return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(cheque)));

                return Result<ChequeDTO>.Success(cheque);
            }
            catch (Exception ex)
            {
                return Result<ChequeDTO>.Failure($"Error al obtener cheque: {ex.Message}");
            }
        }

        public async Task<Result<List<ChequeDTO>?>> ObtenerTodosAsync()
        {
            List<ChequeDTO>? cheques = await _chequeRepository.ObtenerTodosAsync();
            if (cheques != null)
            {
                return Result<List<ChequeDTO>?>.Success(cheques);
            }
            return Result<List<ChequeDTO>?>.Failure(MensajeError.EntidadNoEncontrada(nameof(Cheque), 0));
        }

        public async Task<Result<ChequeDTO>> ActualizarAsync(
            int id,
            DateOnly? fechaIngreso = null,
            int? numeroCheque = null,
            float? monto = null,
            string? banco = null,
            DateOnly? fechaCobro = null,
            string? nombre = null,
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null)
        {
            if (id <= 0)
                return Result<ChequeDTO>.Failure(MensajeError.IdInvalido(id));

            try
            {
                var chequeExistente = await _chequeRepository.ObtenerPorIdAsync(id);

                if (chequeExistente == null)
                    return Result<ChequeDTO>.Failure(MensajeError.objetoNulo(nameof(chequeExistente)));

                // Validaciones básicas de los campos proporcionados
                if (numeroCheque.HasValue && numeroCheque.Value <= 0)
                    return Result<ChequeDTO>.Failure("El número de cheque debe ser mayor a cero");

                if (monto.HasValue && monto.Value <= 0)
                    return Result<ChequeDTO>.Failure("El monto debe ser mayor a cero");

                // Realizar actualización
                bool success = await _chequeRepository.ActualizarAsync(
                    id,
                    fechaIngreso,
                    numeroCheque,
                    monto,
                    banco,
                    fechaCobro,
                    nombre,
                    numeroPersonalizado,
                    fechaVencimiento);

                if (success)
                {
                    ChequeDTO? result = await _chequeRepository.ObtenerPorIdAsync(id);
                    if (result != null)
                        return Result<ChequeDTO>.Success(result);
                }

                return Result<ChequeDTO>.Failure("No se pudo realizar la actualización");
            }
            catch (Exception ex)
            {
                return Result<ChequeDTO>.Failure($"Error al actualizar cheque: {ex.Message}");
            }
        }
        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.IdInvalido(id));

            try
            {
                var cheque = await _chequeRepository.ObtenerPorIdAsync(id);

                if (cheque == null)
                    return Result<bool>.Failure(MensajeError.objetoNulo(nameof(cheque)));

                bool resultado = await _chequeRepository.EliminarAsync(id);

                if (!resultado)
                    return Result<bool>.Failure("No se pudo eliminar el cheque");

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failure($"Error al eliminar el cheque: {e.Message}");
            }
        }
    }
}

