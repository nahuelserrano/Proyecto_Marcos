using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IViajeFleteService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<ViajeFlete>> ActualizarAsync(int id, string? origen, string? destino, float? remito, string? carga, float? km, float? kg, float? tarifa, int? factura, string? cliente, string? nombre_chofer, float? comision, DateOnly? fecha_salida);
        Task<Result<bool>> EliminarAsync(int id);
        Task<Result<int>> InsertarAsync(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string nombre_cliente, string nombre_fletero, string? nombre_chofer, float comision, DateOnly fecha_salida);
        Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnClienteAsync(int id);
        Task<Result<List<ViajeFleteDTO>>> ObtenerViajesDeUnFleteroAsync(string fletero);
    }
}
