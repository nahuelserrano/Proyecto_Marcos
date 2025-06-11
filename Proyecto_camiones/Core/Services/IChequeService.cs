using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IChequeService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<int>> CrearAsync(DateOnly fechaIngreso, int numeroCheque, float monto,
            string banco, DateOnly fechaCobro, string nombre = "", int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null, string entregadoA = "");
        Task<Result<ChequeDTO>> ObtenerPorIdAsync(int id);
        Task<Result<ChequeDTO>> ObtenerPorNumeroAsync(int nroCheque);
        Task<Result<List<ChequeDTO>?>> ObtenerTodosAsync();
        Task<Result<ChequeDTO>> ActualizarAsync(int id, DateOnly? fechaIngreso = null,
            int? numeroCheque = null, float? monto = null, string? banco = null,
            DateOnly? fechaCobro = null, string? nombre = null, int? numeroPersonalizado = null,
            DateOnly? fechaVencimiento = null, string? entregadoA = null);
        Task<Result<bool>> EliminarAsync(int id);
    }
}
