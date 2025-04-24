using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ViajeService
    {
        private readonly ViajeRepository _viajeRepository;
        private readonly CamionService _camionService;
        private readonly ClienteService _clienteService;
        private readonly ChoferService _choferService;

        public ViajeService(
            ViajeRepository viajeRepository,
            CamionService camionService,
            ClienteService clienteService,
            ChoferService choferService)
        {
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository));
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _choferService = choferService ?? throw new ArgumentNullException(nameof(choferService));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            Console.WriteLine("probar service");
            bool result = await _viajeRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<ViajeDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<ViajeDTO>.Failure(MensajeError.idInvalido(id));
            try
            {
                var viaje = await _viajeRepository.ObtenerPorIdAsync(id);
                if (viaje == null)
                    return Result<ViajeDTO>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));
                return Result<ViajeDTO>.Success(viaje);
            }
            catch (Exception ex)
            {
                return Result<ViajeDTO>.Failure($"Error al obtener el viaje: {ex.Message}");
            }
        }

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
            float tarifa)
        {

            try{
                

                ValidadorViaje validador = new ValidadorViaje(fechaInicio, lugarPartida, destino, kg, remito,
                    tarifa, cliente, camion, carga, km);

                Result<bool> resultadoIdRelaciones = validador.ValidarCompleto();


                if (!resultadoIdRelaciones.IsSuccess)
                {
                    return Result<ViajeDTO>.Failure(resultadoIdRelaciones.Error);
                }

                // Revisar que los metodos puedan retornar null si no esxiste el camion o cliente con ese id
                /*
                var clienteResult = await _clienteService.ObtenerPorIdAsync(cliente);
                var camionResult = await _camionService.ObtenerPorIdAsync(camion);

                validador.ValidarExistencia(clienteResult != null, camionResult != null);

                Result<bool> resultadoValidacion = validador.ValidarCompleto();

                if (!resultadoValidacion.IsSuccess)
                {
                    return Result<ViajeDTO>.Failure(resultadoValidacion.Error);
                }
                */
                ViajeDTO viaje = await _viajeRepository.InsertarAsync(
                    fechaInicio, lugarPartida, destino, remito, kg,
                    carga, cliente, camion, km, tarifa);

                if (viaje == null)
                    return Result<ViajeDTO>.Failure("No se pudo crear el viaje en la base de datos");

                return Result<ViajeDTO>.Success(viaje);
            }
            catch (Exception ex)
            {
                return Result<ViajeDTO>.Failure($"Hubo un error al crear el viaje: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var viajes = await _viajeRepository.ObtenerTodosAsync();

                if (viajes.Count == 0)
                    return Result<List<ViajeDTO>>.Success(new List<ViajeDTO>());

                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Ocurrió un error al obtener la lista de viajes: {ex.Message}");
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
           float? tarifa = null)
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.idInvalido(id));

            if (fechaInicio == null && lugarPartida == null && destino == null && remito == null &&
                kg == null && carga == null && cliente == null && camion == null && km == null && tarifa == null)
            {
                return Result<bool>.Failure("No se ha proporcionado ningún campo para actualizar");
            }

            try
            {
                var viajeActual = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viajeActual == null)
                    return Result<bool>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

                bool resultado = await _viajeRepository.ActualizarAsync(
                    id, fechaInicio, lugarPartida, destino, remito,
                    carga, kg, cliente, camion, tarifa, km);

                if (!resultado)
                    return Result<bool>.Failure("No se pudo actualizar el viaje en la base de datos");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al actualizar el viaje: {ex.Message}");
            }
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            if (id <= 0)
                return Result<bool>.Failure("ID de viaje inválido.");

            try
            {
                ViajeDTO viaje = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viaje == null)
                    return Result<bool>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

                bool resultado = await _viajeRepository.EliminarAsync(id);

                if (!resultado)
                    return Result<bool>.Failure("No se pudo eliminar el viaje de la base de datos");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Hubo un error al eliminar el viaje: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(int idCamion)
        {
            if (idCamion <= 0)
            {
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idCamion));
            }

            try
            {
                // Verificar que el camión existe usando el servicio
                if (_camionService != null)
                {
                    var camionResult = await _camionService.ObtenerPorIdAsync(idCamion);
                    if (camionResult == null)
                        return Result<List<ViajeDTO>>.Failure($"El camión especificado no existe: {camionResult}");
                }

                var viajes = await _viajeRepository.ObtenerPorCamionAsync(idCamion);

                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por camión: {ex.Message}");
            }
        }
        public async Task<Result<List<ViajeDTO>>> ObtenerPorClienteAsync(int idCliente)
        {
            if (idCliente < 0)
            {
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idCliente));
            }
            try
            {
                // Verificar que el cliente existe usando el servicio
                
                var clienteResult = await _clienteService.ObtenerPorIdAsync(idCliente);
                if (clienteResult == null)
                    return Result<List<ViajeDTO>>.Failure($"El cliente especificado no existe: {clienteResult}");
                
                var viajes = await _viajeRepository.ObtenerPorClienteAsync(idCliente);
                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por cliente: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(int idChofer)
        {
            if (idChofer <= 0)
            {
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idChofer));
            }
            try
            {
                // Verificar que el chofer existe usando el servicio
                var choferResult = await _choferService.ObtenerPorIdAsync(idChofer);

                if (choferResult == null)
                    return Result<List<ViajeDTO>>.Failure($"El chofer especificado no existe: {choferResult}");

                var viajes = await _viajeRepository.ObtenerPorChoferAsync(choferResult.Value.Nombre);
                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por chofer: {ex.Message}");
            }
        }
    }
}
