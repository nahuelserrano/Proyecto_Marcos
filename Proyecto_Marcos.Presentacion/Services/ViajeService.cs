using Proyecto_Marcos.Presentacion.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransporteApp.Repositories;
using TransporteApp.Utils;

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
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository)); // nameOf() devuelve el nombre del parametro cómo string
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService));
            _choferService = choferService ?? throw new ArgumentNullException(nameof(choferService));
        }

        // Clase Task: representa una operación que está en progreso o que se completará en el futuro, parecidas a las promesas
        public async Task<Result<int>> CrearViaje(Camion viaje)
        {
            if (viaje == null)
                return Result<int>.Failure("¡El viaje no puede ser null!");

            if (viaje.FechaEntrega < DateTime.Now)
                return Result<int>.Failure("No puedes crear viajes en el pasado");

            if (viaje.FechaEntrega <= viaje.FechaInicio)
                return Result<int>.Failure("La fecha de entrega debe ser posterior a la fecha de inicio");


            // Validar el peso de la carga contra la capacidad del camión
            Camion camion = await _camionService.ObtenerCamionAsync(viaje.CamionId);
            if (viaje.KilosCarga > camion.CapacidadMaxima)
                return Result<int>.Failure($"¡La carga excede la capacidad! Máximo permitido: {camion.CapacidadMaxima}kg");


            try
            {
                // Intentar insertar en la base de datos
                int idViaje = await _viajeRepository.InsertarViajeAsync(viaje);
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
                return Result<Viaje>.Failure(ErrorMessages.InvalidId("Viaje"));

            try
            {
                var viaje = await _viajeRepository.ObtenerViajeAsync(id);

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
                var viajes = await _viajeRepository.ObtenerViajesAsync(fechaInicio, fechaFin, choferId, camionId, estado);

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
