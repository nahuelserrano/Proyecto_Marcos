using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface ICamionService
    {
        Task<bool> ProbarConexionAsync();
        Task<List<CamionDTO>> ObtenerTodosAsync();
        Task<Result<int>> CrearAsync(string patente, string nombre);
        Task<CamionDTO> ObtenerPorIdAsync(int id);
        Task<Result<Camion>> ObtenerPorPatenteAsync(string patente);
        Task<Result<CamionDTO>> ActualizarAsync(int id, string? patente, string? nombre);
        Task<Result<string>> EliminarAsync(int id);
        Task<Result<String>> ObtenerChofer(string patente);
    }
}
