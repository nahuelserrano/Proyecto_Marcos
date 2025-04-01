using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ChoferService
    {
        private ChoferRepository _choferRepository;
        private CamionService _camionService;

        public ChoferService(ChoferRepository choferRepository)
        {
            this._choferRepository = choferRepository ?? throw new ArgumentNullException(nameof(choferRepository));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._choferRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<ChoferDTO>> ObtenerPorId(int id)
        {
            if (id <= 0)
                return Result<ChoferDTO>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorId(id);

            if (chofer == null)
                return Result<ChoferDTO>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            return Result<ChoferDTO>.Success(chofer.toDTO());
        }

        internal async Task<Result<bool>> EliminarChofer(int id)
        {
            if (id <= 0) return Result<bool>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorId(id);


            if (chofer == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            this._choferRepository.Eliminar(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> CrearChofer(string nombre, string apellido)
        {

            ValidadorChofer validador = new ValidadorChofer(nombre, apellido);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();
            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);


            try
            {
                int idChofer = await _choferRepository.Insertar(nombre, apellido);
                return Result<int>.Success(idChofer);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
            }
        }

        public async Task<Result<int>> EditarChofer(int id, string nombre = null, string apellido = null)
        {
            if (id <= 0)
                return Result<int>.Failure(MensajeError.idInvalido(id));

            var chofer = await _choferRepository.ObtenerPorId(id);

            if (chofer == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            if (!string.IsNullOrWhiteSpace(nombre))
                chofer.nombre = nombre;

            if (!string.IsNullOrWhiteSpace(apellido))
                chofer.apellido = apellido;

            await _choferRepository.Actualizar(id, nombre, apellido);

            return Result<int>.Success(id);
        }

        public async Task<Result<List<ChoferDTO>>> ObtenerTodos()
        {
            try
            {
                var choferes = await _choferRepository.ObtenerChoferes();
                
                if (choferes == null)
                    return Result<List<ChoferDTO>>.Failure(MensajeError.objetoNulo(nameof(choferes)));

                List<ChoferDTO> choferesDTO = new List<ChoferDTO>();

                foreach (var chofer in choferes)
                {
                    choferesDTO.Add(chofer.toDTO());
                }

                return Result<List<ChoferDTO>>.Success(choferesDTO); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
