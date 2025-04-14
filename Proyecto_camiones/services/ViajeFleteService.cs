using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ViajeFleteService
    {
        private readonly ViajeRepository _viajeRepository;
        private readonly CamionService _camionService;
        private readonly EmpleadoService _empleadoService;

        public ViajeFleteService(
            ViajeRepository viajeRepository,
            CamionService camionService,
            EmpleadoService empleadoService)
        {
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository));
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService));
            _empleadoService = empleadoService ?? throw new ArgumentNullException(nameof(empleadoService));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._viajeRepository.ProbarConexionAsync();
            return result;
        }

        // Clase Task: representa una operación que está en progreso o que se completará en el futuro, parecidas a las promesas
        public async Task<Result<ViajeDTO>> CrearViajeAsync(string destino,
            string lugarPartida,
            float kg,
            int remito,
            float precioPorKilo,
            int chofer,
            int cliente,
            int camion,
            DateOnly fechaInicio,
            string carga,
            float km,
            DateOnly? fechaFacturacion = null)
        {
            ValidadorViaje validador = new ValidadorViaje(
                destino, lugarPartida, kg, remito, precioPorKilo,
                chofer, cliente, camion, fechaInicio, 
                carga, km, fechaFacturacion);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<ViajeDTO>.Failure(resultadoValidacion.Error);

            try
            {
                Viaje viaje = await _viajeRepository.InsertarViajeAsync(
                    destino, lugarPartida, kg, remito, precioPorKilo,
                    chofer, cliente, camion, fechaInicio, fechaFacturacion ?? default,
                    carga, km);

                return Result<ViajeDTO>.Success(viaje.toDTO()); // CORREGIR!
            }
            catch (Exception)
            {
                return Result<ViajeDTO>.Failure("Hubo un error al crear el viaje");
            }
        }

        public async Task<Result<Viaje>> ObtenerViajeAsync(int id)
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
        public async Task<Result<List<ViajeDTO>>> ObtenerViajesAsync(
            DateOnly? fechaInicio = null, // <tipo>? significa que el valor puede ser null
            DateOnly? fechaFin = null,
            int? choferId = null,
            int? camionId = null)
        {
            try
            {
                //var viajes = await _viajeRepository.ObtenerPorFiltro(fechaInicio, fechaFin, choferId, camionId, estado); a implementar en el repositorio
                var viajes = await _viajeRepository.ObtenerTodosAsync();
                List<ViajeDTO> viajesDTO = new List<ViajeDTO>();

                foreach (var viaje in viajes)
                {
                    viajesDTO.Add(viaje.toDTO());
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

        public async Task<Result<bool>> EditarViajeAsync(
           int id,
           string destino = null,
           string lugarPartida = null,
           float? kg = null,
           int? remito = null,
           float? precioPorKilo = null,
           int? empleado = null,
           int? cliente = null,
           int? camion = null,
           DateOnly? fechaInicio = null,
           DateOnly? fechaFacturacion = null,
           string carga = null,
           float? km = null)
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.idInvalido(id));

            // Verificamos que al menos un parámetro sea diferente de null
            if (destino == null && lugarPartida == null && kg == null && remito == null &&
                precioPorKilo == null && empleado == null && cliente == null && camion == null &&
                fechaInicio == null && fechaFacturacion == null && carga == null && km == null)
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
                    destino ?? viajeActual.Destino,
                    lugarPartida ?? viajeActual.LugarPartida,
                    kg ?? viajeActual.Kg,
                    remito ?? viajeActual.Remito,
                    precioPorKilo ?? viajeActual.PrecioPorKilo,
                    empleado ?? viajeActual.Empleado,
                    cliente ?? viajeActual.Cliente,
                    camion ?? viajeActual.Camion,
                    fechaInicio ?? viajeActual.FechaInicio,
                    fechaFacturacion ?? viajeActual.FechaFacturacion,
                    carga ?? viajeActual.Carga,
                    km ?? viajeActual.Km
                );

                // Validamos el viaje modificado
                ValidadorViaje validador = new ValidadorViaje(
                    viajeModificado.Destino,
                    viajeModificado.LugarPartida,
                    viajeModificado.Kg,
                    viajeModificado.Remito,
                    viajeModificado.PrecioPorKilo,
                    viajeModificado.Empleado,
                    viajeModificado.Cliente,
                    viajeModificado.Camion,
                    viajeModificado.FechaInicio,
                    viajeModificado.Carga,
                    viajeModificado.Km,
                    viajeModificado.FechaFacturacion
                );

                // Solo validamos lo que se modificó
                if (fechaInicio != null || fechaFacturacion != null)
                    validador.ValidarFechas();

                if (destino != null || lugarPartida != null)
                    validador.ValidarRuta();

                if (kg != null)
                    validador.ValidarCarga();

                if (precioPorKilo != null || remito != null)
                    validador.ValidarPrecioYRemito();


                Result<bool> resultadoValidacion = validador.ObtenerResultado();

                if (!resultadoValidacion.IsSuccess)
                    return Result<bool>.Failure(resultadoValidacion.Error);

                await _viajeRepository.ActualizarAsync(id, destino, lugarPartida, kg, remito, 
                    precioPorKilo, empleado, cliente, camion, 
                    fechaInicio, fechaFacturacion, carga, km);
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure(MensajeError.errorActualizacion(nameof(Viaje)));
            }
        }

        public async Task<Result<bool>> EliminarViajeAsync(int id)
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

        public async Task<Result<List<ViajeDTO>>> ObtenerViajesPorCamionAsync(int idCamion)
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

        public async Task<Result<List<ViajeDTO>>> ObtenerViajesPorEmpleado(int idEmpleado)
        {
            if (idEmpleado <= 0)
            {
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idEmpleado));
            }

            //var viajes = await _viajeRepository.ObtenerPorChofer(idChofer);
            List<ViajeDTO> viajes = null;

            if (viajes == null || viajes.Count == 0)
            {
                return Result<List<ViajeDTO>>.Failure("No se encontraron viajes para el chofer especificado");
            }
            return Result<List<ViajeDTO>>.Success(viajes);
        }
    }
}
