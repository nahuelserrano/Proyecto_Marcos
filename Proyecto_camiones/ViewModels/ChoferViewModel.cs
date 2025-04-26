using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
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
            this._choferRepository = new ChoferRepository(General.obtenerInstancia());
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
            if (chofer.IsSuccess)
            {
                return Result<ChoferDTO>.Success(chofer.Value);
            }
            else
            {
                return Result<ChoferDTO>.Failure(chofer.Error);
            }
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            Result<bool> resultado = await _choferService.EliminarAsync(id);
            if (resultado.IsSuccess)
            {
                return Result<bool>.Success(resultado.Value);
            }
            else
            {
                return Result<bool>.Failure(resultado.Error);
            }
        }

        public async Task<Result<List<ChoferDTO>>> ObtenerTodosAsync()
        {
            Result<List<ChoferDTO>> choferes = await _choferService.ObtenerTodosAsync();
            if (choferes.IsSuccess)
            {
                return Result<List<ChoferDTO>>.Success(choferes.Value);
            }
            else
            {
                return Result<List<ChoferDTO>>.Failure(choferes.Error);
            }
        }

        public async Task<Result<ChoferDTO>> ActualizarAsync(int id, string nombre)
        {
            Result<ChoferDTO> chofer = await _choferService.ActualizarAsync(id, nombre);
            if (chofer.IsSuccess)
            {
                return Result<ChoferDTO>.Success(chofer.Value);
            }
            else
            {
                return Result<ChoferDTO>.Failure(chofer.Error);
            }
        }
    }
}
