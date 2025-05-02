using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Services;

namespace Proyecto_camiones.Presentacion.Services
{
    public class ViajeService
    {
        private readonly ViajeRepository _viajeRepository;
        private readonly CamionService _camionService;
        private readonly ClienteService _clienteService;
        private readonly ChoferService _choferService;
        private readonly PagoService _pagoService;

        public ViajeService(
            ViajeRepository viajeRepository,
            CamionService camionService,
            ClienteService clienteService,
            ChoferService choferService,
            PagoService pagoService)
        {
            _viajeRepository = viajeRepository ?? throw new ArgumentNullException(nameof(viajeRepository));
            _camionService = camionService ?? throw new ArgumentNullException(nameof(camionService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _choferService = choferService ?? throw new ArgumentNullException(nameof(choferService));
            _pagoService = pagoService ?? throw new ArgumentNullException(nameof(pagoService));
        }

        public async Task<bool> ProbarConexionAsync()
        {
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

        public async Task<Result<int>> CrearAsync(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            float kg,
            string carga,
            int cliente,
            int camion,
            float km,
            float tarifa,
            string nombreChofer,
            double porcentajeChofer)
        {
            
            try{
                ValidadorViaje validador = new ValidadorViaje(fechaInicio, lugarPartida, destino, kg, remito,
                    tarifa, cliente, camion, carga, km);

                Result<bool> resultadoValidarCompleto = validador.ValidarCompleto();


                if (!resultadoValidarCompleto.IsSuccess)
                    return Result<int>.Failure(resultadoValidarCompleto.Error);
                

                // Revisar que los metodos puedan retornar null si no esxiste el camion o cliente con ese id
                
                var clienteResult = await _clienteService.ObtenerPorIdAsync(cliente);
                CamionDTO? camionResult = await _camionService.ObtenerPorIdAsync(camion);

                validador.ValidarExistencia(clienteResult.IsSuccess, camionResult != null);

                Result<bool> resultadoValidacion = validador.ValidarCompleto();

                if (!resultadoValidacion.IsSuccess)
                    return Result<int>.Failure(resultadoValidacion.Error);
                

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var obtenerChoferResult = await _choferService.ObtenerPorNombreAsync(nombreChofer);
                    int idChofer;

                    if (!obtenerChoferResult.IsSuccess)
                    {
                        Result<int> crearChoferResult = await _choferService.CrearAsync(nombreChofer);

                        if (crearChoferResult.IsSuccess)
                            idChofer = crearChoferResult.Value;
                        else
                            return Result<int>.Failure($"Error al crear el chofer: {crearChoferResult.Error}");
                    }
                    else
                        idChofer = obtenerChoferResult.Value.Id;

                    // Crear el viaje
                    int id = await _viajeRepository.InsertarAsync(
                        fechaInicio, lugarPartida, destino, remito, kg,
                        carga, cliente, camion, km, tarifa, nombreChofer, porcentajeChofer);

                    if (id == -1)
                        return Result<int>.Failure("No se pudo crear el viaje en la base de datos");

                    // Crear el pago asociado al viaje
                    float pago_monto = (float)(tarifa * kg * porcentajeChofer);

                    //int idPago = await _pagoService.CrearAsync(idChofer, id, pago_monto);
                    //int idPago = await _pagoService.CrearAsync(idChofer, id, tarifa * kg, porcentajeChofer);


                    //if (idPago <= 0)
                    //    return Result<int>.Failure("No se pudo crear el pago en la base de datos");

                    scope.Complete();
                    return Result<int>.Success(id);
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Hubo un error al crear el viaje: {ex.Message}");
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
           string? lugarPartida = null,
           string? destino = null,
           int? remito = null,
           float? kg = null,
           string? carga = null,
           int? cliente = null,
           int? camion = null,
           float? km = null,
           float? tarifa = null,
           string chofer = null)
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

                // Actualizar el pago asociado al viaje
                //bool seActualizoPago = await _pagoService.ActualizarAsync(id, kg, tarifa, chofer)

                //if (!seActualizoPago)
                //    return Result<bool>.Failure("No se pudo actualizar el pago asociado al viaje");

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
                ViajeDTO? viaje = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viaje == null)
                    return Result<bool>.Failure(MensajeError.NoExisteId(nameof(Viaje), id));

                bool resultado = await _viajeRepository.EliminarAsync(id);

                if (!resultado)
                    return Result<bool>.Failure("No se pudo eliminar el viaje de la base de datos");

                //bool seEliminoPago = await _pagoService.EliminarAsync(id);

                //if (!seEliminoPago)
                //    return Result<bool>.Failure("No se pudo eliminar el pago asociado al viaje");

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
                return Result<List<ViajeDTO>>.Failure(MensajeError.idInvalido(idCamion));

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
        public async Task<Result<List<ViajeMixtoDTO>>> ObtenerPorClienteAsync(int idCliente)
        {
            if (idCliente < 0)
                return Result<List<ViajeMixtoDTO>>.Failure(MensajeError.idInvalido(idCliente));
            
            try
            {
                // Verificar que el cliente existe usando el servicio
                
                var clienteResult = await _clienteService.ObtenerPorIdAsync(idCliente);
                if (clienteResult == null)
                    return Result<List<ViajeMixtoDTO>>.Failure($"El cliente especificado no existe: {clienteResult}");
                
                var viajes = await _viajeRepository.ObtenerPorClienteAsync(idCliente);
                return Result<List<ViajeMixtoDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeMixtoDTO>>.Failure($"Error al obtener viajes por cliente: {ex.Message}");
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

                if (choferResult.Value == null)
                    return Result<List<ViajeDTO>>.Failure($"El chofer especificado no existe: {choferResult}");

                var viajes = await _viajeRepository.ObtenerPorChoferAsync(choferResult.Value.Nombre);
                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por chofer: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(string nombreChofer)
        {
            if (string.IsNullOrWhiteSpace(nombreChofer))
            {
                return Result<List<ViajeDTO>>.Failure("El nombre del chofer no puede estar vacío");
            }

            try
            {
                var viajes = await _viajeRepository.ObtenerPorChoferAsync(nombreChofer);
                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por nombre de chofer: {ex.Message}");
            }
        }
    }
}
