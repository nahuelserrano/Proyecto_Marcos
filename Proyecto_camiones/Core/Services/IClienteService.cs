using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.Core.Services
{
    public interface IClienteService
    {
        Task<bool> ProbarConexionAsync();
        Task<Result<Cliente>> ObtenerPorIdAsync(int id);
        Task<Result<bool>> EliminarAsync(int clienteId);
        Task<Result<int>> InsertarAsync(string nombre);
        Task<Result<Cliente>> ActualizarAsync(int id, string? nombre);
        Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnClienteAsync(string cliente);
        Task<Result<List<Cliente>>> ObtenerTodosAsync();
        Task<Result<Cliente>> ObtenerPorNombreAsync(string nombre);
    }
}
