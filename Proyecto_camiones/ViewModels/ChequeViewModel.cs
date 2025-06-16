using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.ViewModels
{
    public class ChequeViewModel
    {
        private readonly ChequeService _chequeService;

        public ChequeViewModel()
        {
            var chequeRepository = new ChequeRepository();
            _chequeService = new ChequeService(chequeRepository, new ClienteService(new ClienteRepository()));
        }

        public async Task<bool> TestearConexionAsync()
        {
            return await _chequeService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(
            DateOnly fechaIngreso,
            int numeroCheque,
            float monto,
            string banco,
            DateOnly fechaCobro,
            string nombre = "",
            int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null,
            string entregadoA = "") // NUEVO PARÁMETRO - Más importante que saber si existe vida extraterrestre
        {
            if (!await TestearConexionAsync())
                return Result<int>.Failure(MensajeError.errorConexion());

            return await _chequeService.CrearAsync(
                fechaIngreso,
                numeroCheque,
                monto,
                banco,
                fechaCobro,
                nombre,
                numeroPersonalizado,
                fechaVencimiento,
                entregadoA); // PASAR NUEVO PARÁMETRO
        }

        public async Task<Result<ChequeDTO>> ObtenerPorIdAsync(int id)
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure("La conexión no pudo establecerse");

            return await _chequeService.ObtenerPorIdAsync(id);
        }

        public async Task<Result<ChequeDTO>> ObtenerPorNumeroAsync(int num)
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure("La conexión no pudo establecerse");

            var resultado = await _chequeService.ObtenerPorNumeroAsync(num);

            return resultado;
        }


        public async Task<Result<List<ChequeDTO>>> ObtenerTodosAsync()
        {
            if (!await TestearConexionAsync())
                return Result<List<ChequeDTO>>.Failure(MensajeError.errorConexion());

            var cheques = await _chequeService.ObtenerTodosAsync();

            if (!cheques.IsSuccess)
                return Result<List<ChequeDTO>>.Failure("No se pudieron obtener los cheques");

            return cheques;
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
            DateOnly? fechaVencimiento = null,
            string? entregadoA = null) // NUEVO PARÁMETRO
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure(MensajeError.errorConexion());

            return await _chequeService.ActualizarAsync(
                id,
                fechaIngreso,
                numeroCheque,
                monto,
                banco,
                fechaCobro,
                nombre,
                numeroPersonalizado,
                fechaVencimiento,
                entregadoA); // PASAR NUEVO PARÁMETRO
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (!await TestearConexionAsync())
                return Result<bool>.Failure(MensajeError.errorConexion());

            return await _chequeService.EliminarAsync(id);
        }
    }
}