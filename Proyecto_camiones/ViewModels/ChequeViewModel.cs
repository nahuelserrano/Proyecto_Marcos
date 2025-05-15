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
            var dbContext = General.obtenerInstancia();
            var chequeRepository = new ChequeRepository(dbContext);
            _chequeService = new ChequeService(chequeRepository, new ClienteService(new ClienteRepository(General.obtenerInstancia())));
        }

        public async Task<bool> TestearConexionAsync()
        {
            return await _chequeService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(DateOnly fechaIngreso, string banco, int numeroCheque,
                                                 float pesos, string nombre, int numeroPersonal, string entregadoA, DateOnly fechaCobro)
        {
            if (!await TestearConexionAsync())
                return Result<int>.Failure("La conexión no pudo establecerse");

            var resultado = await _chequeService.CrearAsync(fechaIngreso, banco, numeroCheque, pesos, nombre, numeroPersonal, entregadoA, fechaCobro);

            return resultado;
            return Result<int>.Failure("");
        }

        public async Task<Result<ChequeDTO>> ObtenerPorNumeroAsync(string num)
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure("La conexión no pudo establecerse");

            var resultado = await _chequeService.ObtenerPorNumeroAsync(num);

            return resultado;
        }

        public async Task<Result<ChequeDTO>> ObtenerPorIdAsync(int id)
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure("La conexión no pudo establecerse");
            var resultado = await _chequeService.ObtenerPorIdAsync(id);
            return resultado;
        }

        public async Task<Result<List<ChequeDTO>>> ObtenerTodosAsync()
        {
            if (!await TestearConexionAsync())
                return Result<List<ChequeDTO>>.Failure("La conexión no pudo establecerse");

            var cheques = await _chequeService.ObtenerTodosAsync();

            if (cheques == null)
                return Result<List<ChequeDTO>>.Failure("No se pudieron obtener los cheques");

            return Result<List<ChequeDTO>>.Success(cheques);
        }

        public async Task<Result<ChequeDTO>> ActualizarAsync(int id, int? idCliente = null,
                                                            DateOnly? fechaIngreso = null,
                                                            int? numeroCheque = null,
                                                            float? monto = null,
                                                            string banco = null,
                                                            DateOnly? fechaCobro = null)
        {
            if (!await TestearConexionAsync())
                return Result<ChequeDTO>.Failure("La conexión no pudo establecerse");

            var resultado = await _chequeService.ActualizarAsync(id, idCliente, fechaIngreso,
                                                               numeroCheque, monto, banco, fechaCobro);

            return resultado;
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (!await TestearConexionAsync())
                return Result<bool>.Failure("La conexión no pudo establecerse");

            //var resultado = await _chequeService.EliminarAsync(id);

            //return resultado;
            return Result<bool>.Failure("");

        }
    }
}
