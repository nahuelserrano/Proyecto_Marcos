using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Utils;


namespace Proyecto_Marcos.Presentacion.Services
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
            catch (Exception ex)
            {
                // Si algo sale mal, registrar el error y devolver un mensaje amigable
                //Logger.LogError($"Error al crear viaje: {ex.Message}"); 
                return Result<int>.Failure("Hubo un error al crear el viaje");
            }
        }

        public async Task<Result<Viaje>> ObtenerViaje(int id)
        {
            if (id <= 0)
                return Result<Viaje>.Failure(MensajeError.idInvalido("Viaje"));

            try
            {
                var viaje = await _viajeRepository.ObtenerPorId(id);

                if (viaje == null)
                    return Result<Viaje>.Failure($"No se encontró ningún viaje con el ID {id}");

                return Result<Viaje>.Success(viaje);
            }
            catch (Exception ex)
            {
                // Logger.LogError($"Error al obtener viaje {id}: {ex.Message}");
                return Result<Viaje>.Failure($"Ocurrió un error al obtener el viaje con ID {id}");
            }
        }

        // READ - Obtener todos los viajes con filtros opcionales
        public async Task<Result<List<Viaje>>> ObtenerViajes(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? choferId = null,
            int? camionId = null,
            string estado = null)
        {
            try
            {
                var viajes = await _viajeRepository.ObtenerPorFiltro(fechaInicio, fechaFin, choferId, camionId, estado);

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
    }
}
