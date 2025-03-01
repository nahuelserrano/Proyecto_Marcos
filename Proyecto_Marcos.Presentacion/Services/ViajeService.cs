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
        public async Task<Result<int>> CrearViajeAsync(Viaje viaje)
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

        public async Task<Result<int>> getViajeByIdAsync()
        {
            return Result<int>.Failure("Hubo un error al crear el viaje");
        }
    }
}
