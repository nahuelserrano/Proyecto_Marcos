using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.ViewModels;

namespace Proyecto_camiones.Presentacion.Services
{
    public class CamionService
    {
        private CamionRepository _camionRepository;
        private ChoferRepository _choferRepository;
        private ViajeRepository _viajeRepo;


        public CamionService(CamionRepository camionR)
        {
            this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
            this._choferRepository = new ChoferRepository(General.obtenerInstancia());
            this._viajeRepo = new ViajeRepository(General.obtenerInstancia());
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
        public async Task<Result<int>> CrearCamionAsync(string patente, string nombre)
        {

            try
            {
                // Intentar insertar en la base de datos
                Camion response = await _camionRepository.InsertarCamionAsync( patente, nombre);
                if (response != null)
                {
                    int id = await this._choferRepository.InsertarAsync(nombre);

                    if (id != -1)
                    {
                        return Result<int>.Success(response.Id);
                    }

                    return Result<int>.Failure(
                        "Error al insertar el chofer de ese camión, modifiquelo y vuelva a intentar");
                }
                return Result<int>.Failure("Hubo un error al crear el camion");
            }
            catch (Exception ex)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear camion: {ex.Message}"); 
                return Result<int>.Failure("Hubo un error al crear el camion");
            }
        }

        public async Task<CamionDTO> ObtenerPorIdAsync(int id)
        {
            try
            {
                CamionDTO camion = await _camionRepository.ObtenerPorId(id);
                return camion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        internal async Task<Result<CamionDTO>> Actualizar(int id,  string? patente, string? nombre)
        {
            if (id <= 0)
                return Result<CamionDTO>.Failure(MensajeError.idInvalido(id));
            if (patente == null && nombre == null)
                return Result<CamionDTO>.Failure("No se han proporcionado datos para actualizar");
            var camionExistente = await _camionRepository.ObtenerPorId(id);

            if (camionExistente == null) { 
            return Result<CamionDTO>.Failure(MensajeError.objetoNulo(nameof(camionExistente)));
            }

            bool success = await this._camionRepository.Actualizar(id, patente, nombre);
            if (success)
            {
                CamionDTO result = await this._camionRepository.ObtenerPorId(id);
                return Result<CamionDTO>.Success(result);
            }
            return Result<CamionDTO>.Failure("No se pudo realizar la actualización");
        }

        public async Task<Result<string>> Eliminar(int id)
        {
            var viajes = await this._viajeRepo.ObtenerPorFechaYCamionAsync(id);
            if(viajes.Count > 0)
            {
                Console.WriteLine("entramos al if");
                return Result<string>.Failure("No se puede eliminar el camión con el id:" + id + " porque el camión tiene viajes registrados");
            }
            bool success = await this._camionRepository.EliminarCamionAsync(id);
            
            Console.WriteLine("rompimos acá?");
            if (success)
            {
                Console.WriteLine("holu ya se eliminó el camión");
                return Result<string>.Success("el camion con el id " + id + " fue eliminado correctamente");
            }
            return Result<string>.Failure("el camion con el id " + id + " no pudo ser eliminado");
        }
    }
}
