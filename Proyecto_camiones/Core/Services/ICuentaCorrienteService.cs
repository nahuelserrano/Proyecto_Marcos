using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Models;

namespace Proyecto_camiones.Core.Services
{
    public interface ICuentaCorrienteService
    {
        Task<bool> ProbarConexionAsync();
        Task<List<CuentaCorriente>> ObtenerTodosAsync();
        Task<CuentaCorrienteDTO> ObtenerCuentaMasRecienteByClientId(int id);
        Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasByIdClienteAsync(string cliente);
        Task<Result<List<CuentaCorrienteDTO>>> ObtenerCuentasDeUnFletero(string fletero);
        Task<int> Insertar(string? cliente, string? fletero, DateOnly fecha, int nro, float adeuda, float pagado);
        Task<Result<bool>> EliminarAsync(int id);
        Task<Result<CuentaCorrienteDTO>> ActualizarAsync(int id, DateOnly? fecha, int? nroFactura, float? adeuda, float? importe, string? cliente, string? fletero);
    }
}
