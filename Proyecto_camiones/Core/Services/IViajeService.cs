using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IViajeService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<ViajeDTO>> ObtenerPorIdAsync(int id);
        Task<Result<int>> CrearAsync(DateOnly fechaInicio, string lugarPartida, string destino,
            int remito, float kg, string carga, string cliente, string camion, float km,
            float tarifa, string nombreChofer, float porcentajeChofer);
        Task<Result<List<ViajeDTO>>> ObtenerTodosAsync();
        Task<Result<bool>> ActualizarAsync(int id, DateOnly? fechaInicio = null,
            string? lugarPartida = null, string? destino = null, int? remito = null,
            float? kg = null, string? carga = null, string? nombreCliente = null,
            string? patenteCamion = null, float? km = null, float? tarifa = null,
            string? chofer = null, float? porcentaje = null);
        Task<Result<bool>> EliminarAsync(int id);
        Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(string patente);
        Task<Result<List<ViajeMixtoDTO>>> ObtenerPorClienteAsync(int idCliente);
        Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(int idChofer);
        Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(string nombreChofer);
        Task<bool> TienePagoPendiente(int id);
    }
}
