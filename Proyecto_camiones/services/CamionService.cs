using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Proyecto_camiones.DTOs;
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

        //PROBAR CONEXION
        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._camionRepository.ProbarConexionAsync();
            return result;
        }


        //OBTENER TODOS LOS CAMIONES
        public async Task<List<CamionDTO>> ObtenerCamionesAsync()
        {
            List<CamionDTO> camiones = await _camionRepository.ObtenerCamionesAsync();
            if(camiones != null)
            {
                return camiones;
            }
            return new List<CamionDTO>();
        }


        //CREAR CAMION
        public async Task<Result<int>> CrearCamionAsync(float peso, float tara, string patente)
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

        internal async Task<CamionDTO> Actualizar(int id, float? peso_max, float? tara, string? patente)
        {
            bool success = await this._camionRepository.Actualizar(id, peso_max, tara, patente);
            if (success)
            {
                CamionDTO result = await this._camionRepository.ObtenerPorId(id);
                return result;
            }
            return null;
        }
    }
}
