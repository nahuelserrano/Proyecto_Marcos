using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ViajeService
    {
        private readonly ViajeRepository _viajeRepository;
        private readonly CamionService _camionService;
        private readonly ChoferService _choferService;

        public ViajeService(
            ViajeRepository viajeRepository,
            CamionService camionService,
            ChoferService choferService)
        { // Si el valor de alguna de las variables es null, lanza la excepción
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository)); // ?? es el operador que remplaza null por el valor de la derecha
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService)); // nameOf() devuelve el nombre del parametro cómo string
            _choferService = choferService ?? throw new ArgumentNullException(nameof(choferService)); 
        }

        // Clase Task: representa una operación que está en progreso o que se completará en el futuro, parecidas a las promesas
        public async Task<Result<int>> CrearViaje(Viaje viaje)
        {
            ValidadorViaje validador = new ValidadorViaje(viaje);

            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                // Intentar insertar en la base de datos
                int idViaje = await _viajeRepository.Insertar(viaje);
                return Result<int>.Success(idViaje);
            }
            catch (Exception)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear viaje: {ex.Message}"); 
                return Result<int>.Failure("Hubo un error al crear el viaje");
            }
        }

        public async Task<Result<Viaje>> ObtenerViaje(int id)
        {
            if (id <= 0)
                return Result<Viaje>.Failure(MensajeError.idInvalido(id));

            try
            {
                var viaje = await _viajeRepository.ObtenerPorId(id);

                if (viaje == null)
                    return Result<Viaje>.Failure($"No se encontró ningún viaje con el ID {id}");

                return Result<Viaje>.Success(viaje);
            }
            catch (Exception)
            {
                // Logger.LogError($"Error al obtener viaje {id}: {ex.Message}");
                return Result<Viaje>.Failure($"Ocurrió un error al obtener el viaje con ID {id}");
            }
        }

        // READ - Obtener todos los viajes con filtros opcionales
        public async Task<Result<List<Viaje>>> ObtenerViajes(
            DateTime? fechaInicio = null, // <tipo>? significa que el valor puede ser null
            DateTime? fechaFin = null,
            int? choferId = null,
            int? camionId = null,
            string estado = null)
        {
            try
            {
                //var viajes = await _viajeRepository.ObtenerPorFiltro(fechaInicio, fechaFin, choferId, camionId, estado); a implementar en el repositorio
                var viajes = await _viajeRepository.ObtenerTodos();

                if (viajes == null || viajes.Count == 0)
                    return Result<List<Viaje>>.Success(new List<Viaje>()); // Devolver lista vacía, no es un error

                return Result<List<Viaje>>.Success(viajes);
            }
            catch (Exception ex)
            {
                // Logger.LogError($"Error al obtener lista de viajes: {ex.Message}");
                return Result<List<Viaje>>.Failure("Ocurrió un error al obtener la lista de viajes");
            }
        }

        public async Task<Result<bool>> EditarViaje(int id,
            DateTime? fechaInicio = null,
            DateTime? fechaEntrega = null,
            int? choferId = null,
            int? camionId = null,
            string estado = null)
        {


            if (id <= 0)
                return Result<bool>.Failure(MensajeError.idInvalido(id));

            if (fechaInicio == null && fechaEntrega == null && choferId == null && camionId == null && estado == null)
                return Result<bool>.Failure("No se especificó ningún campo para editar");

            Viaje viaje =  await _viajeRepository.ObtenerPorId(id);

            if (viaje == null)
                return Result<bool>.Failure(MensajeError.objetoNulo(nameof(viaje)));

            ValidadorViaje validador = new ValidadorViaje(viaje);

            if (fechaInicio != null)
            {
                viaje.FechaInicio = (DateTime)fechaInicio;
                validador.ValidarFechas();
            }

            if (fechaEntrega != null)
            {
                viaje.FechaEntrega = (DateTime)fechaEntrega;
                validador.ValidarFechas();
            }

            if (choferId != null)
            {
                Result<Chofer> r = await _choferService.ObtenerPorId((int) choferId);
                viaje.Chofer = r.Value;
                validador.ValidarEntidadesRelacionadas();
            }

            if (camionId != null)
            {
                Result<Camion> r = await _camionService.ObtenerPorId((int)camionId);
                viaje.Camion = r.Value;
                validador.ValidarEntidadesRelacionadas();
            }

            if (estado != null)
            {
                Viaje r =  await _viajeRepository.ObtenerPorId(id);
                r.Estado = estado;
                validador.ValidarEntidadesRelacionadas();
            }

            Result<bool> resultadoValidacion = validador.ObtenerResultado();

            if (!resultadoValidacion.IsSuccess)
                return Result<bool>.Failure(resultadoValidacion.Error);

            try
            {
                // Intentar insertar en la base de datos
                await _viajeRepository.Actualizar(id, viaje);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Logger.LogError($"Error al editar viaje {id}: {ex.Message}");
                return Result<bool>.Failure("Hubo un error al editar el viaje");
            }
        }

        public async Task<Result<bool>> EliminarViaje(int id)
        {
            if (id <= 0)
                return Result<bool>.Failure("ID de viaje inválido.");

            Viaje viaje = _viajeRepository.ObtenerPorId(id).Result;

            if (viaje == null)
                return Result<bool>.Failure("El viaje no existe.");

            try
            {
                // Intentar insertar en la base de datos
                await _viajeRepository.Eliminar(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Logger.LogError($"Error al eliminar viaje {id}: {ex.Message}");
                return Result<bool>.Failure("Hubo un error al eliminar el viaje");
            }
        }
    }
}
