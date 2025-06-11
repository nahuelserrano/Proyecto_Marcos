using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Proyecto_camiones.Core.Services;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;

namespace Proyecto_camiones.Presentacion.Services
{
    public class CamionService : ICamionService
    {
        private CamionRepository _camionRepository;
        private ChoferRepository _choferRepository;
        private ViajeRepository _viajeRepo;
        private IPagoService _pagoService;


        public CamionService(
            CamionRepository camionRepository,
            ChoferRepository choferRepository,
            ViajeRepository viajeRepository,
            IPagoService pagoService)
        {
            _camionRepository = camionRepository ?? throw new ArgumentNullException(nameof(camionRepository));
            _choferRepository = choferRepository ?? throw new ArgumentNullException(nameof(choferRepository));
            _viajeRepo = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository));
            _pagoService = pagoService ?? throw new ArgumentNullException(nameof(pagoService));
        }

        //PROBAR CONEXION
        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._camionRepository.ProbarConexionAsync();
            return result;
        }


        //OBTENER TODOS LOS CAMIONES
        public async Task<List<CamionDTO>> ObtenerTodosAsync()
        {
            List<CamionDTO> camiones = await _camionRepository.ObtenerTodosAsync();
            if(camiones != null)
            {
                return camiones;
            }
            return new List<CamionDTO>();
        }


        //CREAR CAMION
        public async Task<Result<int>> CrearAsync(string patente, string nombre)
        {

            try
            {
                // Intentar insertar en la base de datos
                Camion response = await _camionRepository.InsertarAsync(patente, nombre);
                if (response != null)
                {
                    int id = await this._choferRepository.InsertarAsync(nombre);

                    if (id != -1)
                    {
                        return Result<int>.Success(response.Id);
                    }

                    return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
                }
               return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
            }
            catch (Exception ex)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear camion: {ex.Message}"); 
                return Result<int>.Failure(MensajeError.ErrorCreacion("camión"));
            }
        }

        public async Task<CamionDTO> ObtenerPorIdAsync(int id)
        {
            try
            {
                CamionDTO camion = await _camionRepository.ObtenerPorIdAsync(id);
                return camion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<Camion>> ObtenerPorPatenteAsync(string patente)
        {
            try
            {
                Camion? camion = await _camionRepository.ObtenerPorPatenteAsync(patente);

                if (camion == null)
                    return Result<Camion>.Failure(MensajeError.EntidadNoEncontrada(nameof(Camion), 0));

                return Result<Camion>.Success(camion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<CamionDTO>> ActualizarAsync(int id,  string? patente, string? nombre)
        {
            if (id <= 0)
                return Result<CamionDTO>.Failure(MensajeError.IdInvalido(id));
            if (patente == null && nombre == null)
                return Result<CamionDTO>.Failure(MensajeError.ErrorActualizacion("camión"));
            var camionExistente = await _camionRepository.ObtenerPorIdAsync(id);

            if (camionExistente == null) { 
                return Result<CamionDTO>.Failure(MensajeError.objetoNulo(nameof(camionExistente)));
            }

            bool success = await this._camionRepository.ActualizarAsync(id, patente, nombre);
            if (success)
            {
                CamionDTO result = await this._camionRepository.ObtenerPorIdAsync(id);
                return Result<CamionDTO>.Success(result);
            }
            return Result<CamionDTO>.Failure(MensajeError.objetoNulo(nameof(camionExistente)));
        }

        public async Task<Result<string>> EliminarAsync(int id)
        {
            CamionDTO? c = await this._camionRepository.ObtenerPorIdAsync(id);
            if(c == null)
            {
                return Result<string>.Failure("No se encontró un camión con ese id");
            }

            Chofer? ch = await this._choferRepository.ObtenerPorNombreAsync(c.Nombre_Chofer);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (ch != null)
                    await this._choferRepository.EliminarAsync(ch.Id);
                
                var  viajesDelCamion = await _viajeRepo.ObtenerPorCamionAsync(id, c.Patente);

                if (viajesDelCamion == null)
                    return Result<string>.Failure(MensajeError.ErrorEliminacion("viaje"));

                foreach (ViajeDTO viaje in viajesDelCamion)
                {
                    Result<Pago> pResult = await _pagoService.ObtenerPorIdViajeAsync(viaje.Id);

                    if(!pResult.IsSuccess)
                        return Result<string>.Failure(MensajeError.EntidadNoEncontrada("pago", viaje.Id));

                    Pago p = pResult.Value;

                    // Si el pago no está pagado, no se puede eliminar el viaje
                    if (!p.Pagado)
                        return Result<string>.Failure("El chofer asociado al camión tiene viajes pendientes a cobrar");
                }

                await this._camionRepository.EliminarAsync(id);

                scope.Complete();
                return Result<string>.Success(MensajeError.EliminacionExitosa("camión"));
            }
        }

        public async Task<Result<String>> ObtenerChofer(string patente)
        {
            Camion camion = await this._camionRepository.ObtenerPorPatenteAsync(patente);
            if(camion != null)
            {
                return Result<String>.Success(camion.nombre_chofer);
            }
            else
            {
                return Result<String>.Failure("No existe camión con esa patente");
            }
        }
    }
}
