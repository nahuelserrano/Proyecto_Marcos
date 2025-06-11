using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IChoferService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<ChoferDTO>> ObtenerPorIdAsync(int id);
        Task<Result<bool>> EliminarAsync(int id);
        Task<Result<int>> CrearAsync(string nombre);
        Task<Result<ChoferDTO>> ActualizarAsync(int id, string nombre);
        Task<Result<List<ChoferDTO>>> ObtenerTodosAsync();
        Task<Result<Chofer?>> ObtenerPorNombreAsync(string nombre);
    }
}
