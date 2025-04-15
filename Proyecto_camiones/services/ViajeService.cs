using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ViajeService
    {
        private readonly ViajeRepository _viajeRepository;
        private readonly CamionService _camionService;
        //private readonly EmpleadoService _empleadoService;

        public ViajeService(
            ViajeRepository viajeRepository,
            CamionService camionService)
        {
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository));
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._viajeRepository.ProbarConexionAsync();
            return result;
        }

        // Clase Task: representa una operación que está en progreso o que se completará en el futuro, parecidas a las promesas
        public async Task<Result<ViajeDTO>> CrearAsync(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            float kg,
            string carga,
            int cliente,
            int camion,
            float km,
            float tarifa
            )
        {
            ValidadorViaje validador = new ValidadorViaje(fechaInicio, lugarPartida, destino, kg, remito, 
                tarifa, cliente, camion, carga, km);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<ViajeDTO>.Failure(resultadoValidacion.Error);

            try
            {
                Viaje viaje = await _viajeRepository.InsertarAsync(
                    fechaInicio, lugarPartida, destino, remito, kg, 
                    carga, cliente, camion, km, tarifa
                    );

                return Result<ViajeDTO>.Success(viaje.toDTO("implementar", "implementar")); // CORREGIR!
            }
            catch (Exception)
            {
                return Result<ViajeDTO>.Failure("Hubo un error al crear el viaje");
            }
        }

        public async Task<Result<Viaje>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<Viaje>.Failure(MensajeError.idInvalido(id));

            try
            {
                var viaje = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viaje == null)
                    return Result<Viaje>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

                return Result<Viaje>.Success(viaje);
            }
            catch (Exception)
            {
                // Logger.LogError($"Error al obtener viaje {id}: {ex.Message}");
                return Result<Viaje>.Failure($"Ocurrió un error al obtener el viaje con ID {id}");
            }
        }

        // READ - Obtener todos los viajes con filtros opcionales
        public async Task<Result<List<ViajeDTO>>> ObtenerTodosAsync()
        {
            try
            {
                //var viajes = await _viajeRepository.ObtenerPorFiltro(fechaInicio, fechaFin, choferId, camionId, estado); a implementar en el repositorio
                var viajes = await _viajeRepository.ObtenerTodosAsync();
                List<ViajeDTO> viajesDTO = new List<ViajeDTO>();

                foreach (var viaje in viajes)
                {
                    //viajesDTO.Add(viaje.toDTO());
                }

                if (viajes == null || viajes.Count == 0)
                    return Result<List<ViajeDTO>>.Success(new List<ViajeDTO>()); // Devolver lista vacía, no es un error

                return Result<List<ViajeDTO>>.Success(viajesDTO);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");
            }
        }

        public async Task<Result<bool>> ActualizarAsync(
           int id,
           DateOnly? fechaInicio = null,
           string lugarPartida = null,
           string destino = null,
           int? remito = null,
           float? kg = null,
           string carga = null,
           int? cliente = null,
           int? camion = null,
           float? km = null,
           float? tarifa = null
           )
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.idInvalido(id));

            // Verificamos que al menos un parámetro sea diferente de null
            if (destino == null && lugarPartida == null && kg == null && remito == null &&
                tarifa == null  && cliente == null && camion == null &&
                fechaInicio == null && carga == null && km == null)
            {
                return Result<bool>.Failure("No se establecio ningún campo a editar");
            }

            try
            {
                var viajeActual = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viajeActual == null)
                    return Result<bool>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

                // Actualizamos con los valores nuevos o mantenemos los anteriores
                var viajeModificado = new Viaje(
                    fechaInicio ?? viajeActual.FechaInicio,
                    lugarPartida ?? viajeActual.LugarPartida,
                    destino ?? viajeActual.Destino,
                    remito ?? viajeActual.Remito,
                    kg ?? viajeActual.Kg,
                    carga ?? viajeActual.Carga,
                    cliente ?? viajeActual.Cliente,
                    camion ?? viajeActual.Camion,
                    km ?? viajeActual.Km,
                    tarifa ?? viajeActual.Tarifa
                    );

                // Validamos el viaje modificado
                ValidadorViaje validador = new ValidadorViaje(
                    viajeModificado.FechaInicio,
                    viajeModificado.LugarPartida,
                    viajeModificado.Destino,
                    viajeModificado.Kg,
                    viajeModificado.Remito,
                    viajeModificado.Tarifa,
                    viajeModificado.Cliente,
                    viajeModificado.Camion,
                    viajeModificado.Carga,
                    viajeModificado.Km
                    );

                // Solo validamos lo que se modificó

                if (destino != null || lugarPartida != null)
                    validador.ValidarRuta();

                if (kg != null)
                    validador.ValidarCarga();

                if (tarifa != null || remito != null)
                    validador.ValidarPrecioYRemito();


                Result<bool> resultadoValidacion = validador.ObtenerResultado();

                if (!resultadoValidacion.IsSuccess)
                    return Result<bool>.Failure(resultadoValidacion.Error);

                await _viajeRepository.ActualizarAsync(id, fechaInicio, lugarPartida, destino, remito, 
                    carga, kg, cliente, camion, tarifa, km);
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure(MensajeError.errorActualizacion(nameof(Viaje)));
            }
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
                return Result<bool>.Failure("ID de viaje inválido.");

            Viaje viaje = _viajeRepository.ObtenerPorIdAsync(id).Result;

            if (viaje == null)
                return Result<bool>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

            try
            {
                // Intentar insertar en la base de datos
                await _viajeRepository.EliminarAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Hubo un error al eliminar el viaje");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(int idCamion)
        {
            if (idCamion <= 0)
            {
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idCamion));
            }

            //var viajes = await _viajeRepository.ObtenerPorCamion(idCamion);
            List<ViajeDTO> viajes = null;

            if (viajes == null || viajes.Count == 0)
            {
                return Result<List<ViajeDTO>>.Failure("No se encontraron viajes para el camión especificado");
            }

            return Result<List<ViajeDTO>>.Success(viajes);
        }
    }
}
