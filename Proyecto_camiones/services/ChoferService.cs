using System;
using System.Threading.Tasks;
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

        public async Task<Result<Chofer>> ObtenerPorId(int id)
        {
            if (id <= 0)
                return Result<Chofer>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorId(id);

            if (chofer == null)
                return Result<Chofer>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            return Result<Chofer>.Success(chofer);
        }

        internal async Task<Result<bool>> EliminarChoferAsync(int id)
        {
            if (id <= 0) return Result<bool>.Failure(MensajeError.idInvalido(id));

            Chofer chofer = await this._choferRepository.ObtenerPorId(id);


            if (chofer == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(chofer)));

            this._choferRepository.Eliminar(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> CrearChoferAsync(Chofer chofer)
        {

            ValidadorChofer validador = new ValidadorChofer(chofer);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();
            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);


            try
            {
                int idChofer = await _choferRepository.Insertar(chofer);
                return Result<int>.Success(idChofer);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
            }
        }

        public async Task<Result<int>> EditarChoferAsync(int id, string nombre = null, String apellido = null)
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

            await _choferRepository.Actualizar(chofer);

            return Result<int>.Success(id);
        }
    }
}
