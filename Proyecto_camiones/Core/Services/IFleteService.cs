using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IFleteService
    {
        Task<bool> TestearConexion();
        Task<Result<int>> InsertarFletero(string nombre);
        Task<Result<Flete>> ObtenerPorNombreAsync(string nombre);
        Task<Result<List<Flete>>> ObtenerTodosAsync();
        Task<Result<Flete>> ObtenerPorIdAsync(int id);
        Task<Result<bool>> EliminarAsync(int id);
        Task<Result<Flete>> ActualizarAsync(int id, string? nombre);
    }
}
