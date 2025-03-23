using System;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class CamionService
    {
        private CamionRepository _camionRepository;


        public CamionService(CamionRepository camionR)
        {
            this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
        }

        //public async Task<Result<Camion>> ObtenerPorId(int id)
        //{
        //    if (id <= 0)
        //        return Result<Camion>.Failure(MensajeError.idInvalido(id));

        //    Camion camion = await this._camionRepository.ObtenerPorId(id);

        //    if (camion == null)
        //        return Result<Camion>.Failure(MensajeError.objetoNulo(nameof(camion)));

        //    return Result<Camion>.Success(camion);
        //}

        //internal async Task<Result<bool>> Eliminar(int camionId)
        //{
        //    if (camionId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(camionId));

        //    Camion camion = await this._camionRepository.ObtenerPorId(camionId);

        //    if (camion == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(camion)));

        //    _camionRepository.Eliminar(camionId);

        //    return Result<bool>.Success(true);

        //}

        public async Task<Result<int>> CrearcamionAsync(float peso, float tara, string patente)
        {
            ValidadorCamion validador = new ValidadorCamion(peso, tara, patente);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                // Intentar insertar en la base de datos
                Camion response = await _camionRepository.InsertarCamionAsync(peso, tara, patente);
                if (response != null)
                {
                    return Result<int>.Success(response.Id);
                }
                else
                {
                    return Result<int>.Failure("El camion no pudo ser incertado");
                }
            }
            catch (Exception ex)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear camion: {ex.Message}"); 
                return Result<int>.Failure("Hubo un error al crear el camion");
            }
        }

        //public async Task<Result<int>> Actualizar(int id, float? capacidadMax = null, float? tara = null, string patente = null)
        //{
        //    if (id <= 0)
        //        return Result<int>.Failure("ID de vehículo inválido.");

        //    var vehiculoExistente = await _camionRepository.ObtenerPorId(id);

        //    if (vehiculoExistente == null)
        //        return Result<int>.Failure(MensajeError.objetoNulo(nameof(vehiculoExistente)));

        //    if (capacidadMax.HasValue)
        //        vehiculoExistente.CapacidadMax = capacidadMax.Value;

        //    if (tara.HasValue)
        //        vehiculoExistente.Tara = tara.Value;

        //    if (!string.IsNullOrWhiteSpace(patente))
        //        vehiculoExistente.Patente = patente;

        //    await _camionRepository.Actualizar(vehiculoExistente);

        //    return Result<int>.Success(id);
        //}
    }
}
