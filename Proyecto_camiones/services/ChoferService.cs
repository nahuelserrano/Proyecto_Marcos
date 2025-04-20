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

        public ChoferService(ChoferRepository choferRepository)
        {
            this._choferRepository = choferRepository ?? throw new ArgumentNullException(nameof(choferRepository));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._choferRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<ChoferDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<ChoferDTO>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorIdAsync(id);

            if (chofer == null)
                return Result<ChoferDTO>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            return Result<ChoferDTO>.Success(chofer.toDTO());
        }

        internal async Task<Result<bool>> EliminarAsync(int id)
        {
            if (id <= 0) return Result<bool>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorIdAsync(id);


            if (chofer == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            this._choferRepository.EliminarAsync(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<ChoferDTO>> CrearAsync(string nombre)
        {

            ValidadorChofer validador = new ValidadorChofer(nombre);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();
            if (!resultadoValidacion.IsSuccess)
                return Result<ChoferDTO>.Failure(resultadoValidacion.Error);


            try
            {
                Chofer chofer = await _choferRepository.InsertarAsync(nombre);

                if (chofer == null)
                {
                    return null;
                }

                return Result<ChoferDTO>.Success(chofer.toDTO());
            }
            catch (Exception ex)
            {
                return Result<ChoferDTO>.Failure("Hubo un error al crear el chofer: " + ex.Message);
            }
        }

        public async Task<Result<ChoferDTO>> ActualizarAsync(int id, string nombre = null)
        {
            if (id <= 0)
                return Result<ChoferDTO>.Failure(MensajeError.idInvalido(id));

            var chofer = await _choferRepository.ObtenerPorIdAsync(id);

            if (chofer == null)
                return Result<ChoferDTO>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            if (!string.IsNullOrWhiteSpace(nombre))
                chofer.Nombre = nombre;

            await _choferRepository.ActualizarAsync(id, nombre);

            return Result<ChoferDTO>.Success(chofer.toDTO());
        }

        public async Task<Result<List<ChoferDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var choferes = await _choferRepository.ObtenerTodosAsync();

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
