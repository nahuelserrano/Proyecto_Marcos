using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IPagoService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<Pago>> ObtenerPorIdViajeAsync(int idViaje);
        Task<int> CrearAsync(int id_chofer, int id_viaje, float monto_pagado);
        Task<Result<bool>> ActualizarAsync(int id_chofer, int id_viaje, float monto_pagado);
        Task<Result<bool>> ModificarEstado(int id_chofer, DateOnly desde, DateOnly hasta, int? id_Sueldo, bool pagado = true);
        Task<float> ObtenerSueldoCalculado(int id_chofer, DateOnly calcularDesde, DateOnly calcularHasta);
    }
}
