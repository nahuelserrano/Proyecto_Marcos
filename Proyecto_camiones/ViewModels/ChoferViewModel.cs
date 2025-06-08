using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.ViewModels
{
    class ChoferViewModel
    {
        private ChoferRepository _choferRepository;
        private ChoferService _choferService;

        public ChoferViewModel()
        {
            this._choferRepository = new ChoferRepository();
            this._choferService = new ChoferService(_choferRepository);
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._choferRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<int>> CrearAsync(string nombre)
        {
            Result<int> result = await _choferService.CrearAsync(nombre);
            return result;
        }

        public async Task<Result<ChoferDTO>> ObtenerPorIdAsync(int id)
        {
            Result<ChoferDTO> chofer = await _choferService.ObtenerPorIdAsync(id);
            return chofer;
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            Result<bool> resultado = await _choferService.EliminarAsync(id);
            return resultado;
        }

        public async Task<Result<List<ChoferDTO>>> ObtenerTodosAsync()
        {
            Result<List<ChoferDTO>> choferes = await _choferService.ObtenerTodosAsync();
            return choferes;
        }

        public async Task<Result<ChoferDTO>> ActualizarAsync(int id, string nombre)
        {
            Result<ChoferDTO> chofer = await _choferService.ActualizarAsync(id, nombre);
            return chofer;
        }

        public async Task<Result<Chofer>> ObetenerPorNombreAsync(string nombre)
        {
            Result<Chofer> chofer = await _choferService.ObtenerPorNombreAsync(nombre);
            return chofer;
        }
    }
}
