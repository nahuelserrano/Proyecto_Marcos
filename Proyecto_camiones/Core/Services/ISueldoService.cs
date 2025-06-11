using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface ISueldoService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string? patenteCamion, string? nombreChofer);
        Task<Result<bool>> EliminarAsync(int sueldoId);
        Task<Result<int>> CrearAsync(string nombre_chofer, DateOnly pagoDesde, DateOnly pagoHasta, DateOnly? fecha_pagado, string patenteCamion);
        Task<Result<SueldoDTO>> marcarPagado(int id);
        Task<Result<SueldoDTO>> ActualizarAsync(int id, float? monto = null, int? Id_Chofer = null, DateOnly? pagadoDesde = null, DateOnly? pagadoHasta = null, DateOnly? FechaPago = null);
    }
}
