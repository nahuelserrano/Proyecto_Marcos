using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
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
        //private int Porcentaje = 100;
        //private int Tonelada = 1000;

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
                return Result<ViajeDTO>.Failure(MensajeError.IdInvalido(id));
            try
            {
                var viaje = await _viajeRepository.ObtenerPorIdAsync(id);
                if (viaje == null)
                    return Result<ViajeDTO>.Failure(MensajeError.EntidadNoEncontrada(nameof(Viaje), id));
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
            string cliente,
            string camion,
            float km,
            float tarifa,
            string nombreChofer,
            float porcentajeChofer)
        { 

            try
            {
                ValidadorViaje validador = new ValidadorViaje(fechaInicio, lugarPartida, destino, kg, remito,
                    tarifa, cliente, camion, carga, km, nombreChofer, porcentajeChofer);

                Result<bool> resultadoValidarCompleto = validador.ValidarCompleto();


                if (!resultadoValidarCompleto.IsSuccess)
                    return Result<int>.Failure(resultadoValidarCompleto.Error);
                

                // Revisar que los metodos puedan retornar null si no esxiste el camion o cliente con ese id
                
                var clienteResult = await _clienteService.ObtenerPorNombreAsync(cliente);
                var camionResult = await _camionService.ObtenerPorPatenteAsync(camion);

                validador.ValidarExistencia(clienteResult.IsSuccess, camionResult.IsSuccess);

                Result<bool> resultadoValidacion = validador.ObtenerResultado();

                if (!resultadoValidacion.IsSuccess)
                    return Result<int>.Failure(resultadoValidacion.Error);
                
                int idCliente = clienteResult.Value.Id;
                int idCamion = camionResult.Value.Id;

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    int idChofer = -1;

                    if(nombreChofer != null)
                    {
                        Result<int> crearChoferResult = await _choferService.CrearAsync(nombreChofer);

                        if (crearChoferResult.IsSuccess)
                            idChofer = crearChoferResult.Value;
                        else
                            return Result<int>.Failure(MensajeError.ErrorCreacion(nameof(Chofer)));
                    }

                    else
                    {
                        Result<String> obtenerChoferResult = await _camionService.ObtenerChofer(camion);

                        if (!obtenerChoferResult.IsSuccess)
                        {
                            return Result<int>.Failure("No se pudo crear el viaje ya que el camión no tiene un chofer asignado y no se ha adjuntado un chofer en el formulario");
                        }
                        else
                        {
                            nombreChofer = obtenerChoferResult.Value;
                            Result<Chofer> c = await this._choferService.ObtenerPorNombreAsync(obtenerChoferResult.Value);
                            if(c.IsSuccess)
                            {
                                idChofer = c.Value.Id;
                            }
                        }
                    }

                    // Crear el viaje
                    int id = await _viajeRepository.InsertarAsync(
                        fechaInicio, lugarPartida, destino, remito, kg,
                        carga, idCliente, idCamion, km, tarifa, nombreChofer, porcentajeChofer);

                    if (id == -1)
                        return Result<int>.Failure(MensajeError.ErrorCreacion(nameof(Viaje)));

                    // Crear el pago asociado al viaje
                    float pagoMonto = tarifa * kg * porcentajeChofer;
                   // *Tonelada / Porcentaje

                    if (idChofer != -1)
                    {
                        int idPago = await _pagoService.CrearAsync(idChofer, id, pagoMonto);

                        if (idPago <= 0)
                            return Result<int>.Failure(MensajeError.ErrorCreacion(nameof(Pago)));
                    }

                    scope.Complete();
                    return Result<int>.Success(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error al crear el viaje");
                return Result<int>.Failure($"Hubo un error al crear el viaje: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var viajes = await _viajeRepository.ObtenerTodosAsync();

                if (viajes == null)
                    return Result<List<ViajeDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");

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
           string? nombreCliente = null,
           string? patenteCamion = null,
           float? km = null,
           float? tarifa = null,
           string? chofer = null,
           float? porcentaje = null)
        {
            if (id <= 0)
                return Result<bool>.Failure(MensajeError.IdInvalido(id));

            if (fechaInicio == null && lugarPartida == null && destino == null && remito == null && kg == null && carga == null
                 && nombreCliente == null && patenteCamion == null && km == null && tarifa == null && chofer == null && porcentaje == null)
            {
                return Result<bool>.Failure("No se ha proporcionado ningún campo para actualizar");
            }

            try
            {
                var viajeActual = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viajeActual == null)
                    return Result<bool>.Failure(MensajeError.EntidadNoEncontrada(nameof(Viaje), id));

                int? idCliente = null, idCamion = null, idChofer = null;
                

                // Verificar que el cliente existe usando el servicio
                if (nombreCliente != null)
                {
                    var clienteResult = await _clienteService.ObtenerPorNombreAsync(nombreCliente);

                    if (!clienteResult.IsSuccess)
                        return Result<bool>.Failure($"El cliente especificado no existe: {clienteResult}");

                    idCliente = clienteResult.Value.Id;
                }

                // Verificar que el camión existe usando el servicio
                if (patenteCamion != null)
                {
                    var camionResult = await _camionService.ObtenerPorPatenteAsync(patenteCamion);

                    if (!camionResult.IsSuccess)
                        return Result<bool>.Failure($"El camión especificado no existe: {camionResult}");

                    idCamion = camionResult.Value.Id;
                }

                if (chofer != null)
                {

                    var obtenerChoferResult = await _choferService.ObtenerPorNombreAsync(chofer);

                    if (!obtenerChoferResult.IsSuccess)
                        return Result<bool>.Failure(MensajeError.ErrorCreacion(nameof(Chofer)));

                    idChofer = obtenerChoferResult.Value.Id;
                    chofer = obtenerChoferResult.Value.Nombre;
                }
                else
                {
                    var choferActual = await _choferService.ObtenerPorNombreAsync(viajeActual.NombreChofer) ;

                    if(choferActual.IsSuccess)
                        idChofer = choferActual.Value.Id;
                    else
                        return Result<bool>.Failure(MensajeError.EntidadNoEncontrada(nameof(Chofer), id));
                }

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    bool resultado = await _viajeRepository.ActualizarAsync(
                        id, fechaInicio, lugarPartida, destino, remito,
                        carga, kg, idCliente, idCamion, tarifa, km, chofer, porcentaje);

                    if (!resultado)
                        return Result<bool>.Failure(MensajeError.ErrorActualizacion(nameof(Viaje)) + " no hubo actualización desde el repository");

                    float tarifaActualizada = 0f, kgActualizado = 0f, porcentajeActualizado = 0f;

                    if (tarifa != null && tarifa > 0)
                        viajeActual.Tarifa = (float) tarifa;

                    if (kg != null && kg > 0)
                        viajeActual.Kg = (float)kg;

                    if (porcentaje != null && porcentaje > 0 && porcentaje < 1)
                        viajeActual.PorcentajeChofer = (float)porcentaje;

                    // Actualizar el pago asociado al viaje
                    var pagoResult = await _pagoService.ActualizarAsync((int)idChofer, id,  viajeActual.GananciaChofer);

                    if (!pagoResult.IsSuccess)
                        return Result<bool>.Failure($"No se pudo actualizar el pago asociado al viaje: {pagoResult.Error}");

                    scope.Complete();
                    return Result<bool>.Success(true);
                }
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
                var viaje = await _viajeRepository.ObtenerPorIdAsync(id);

                if (viaje == null)
                    return Result<bool>.Failure(MensajeError.EntidadNoEncontrada(nameof(Viaje), id));


                Result<Pago> pago = await _pagoService.ObtenerPorIdViajeAsync(id);

                if (pago.Value == null)
                    return Result<bool>.Failure("No se encontró el pago asociado al viaje.");

                if (pago.Value.Pagado)
                    return Result<bool>.Failure("El pago asociado a este viaje ya esta pagado, por tanto no se puede eliiminar.");

                bool seEliminoViaje = await _viajeRepository.EliminarAsync(id);

                if (!seEliminoViaje)
                    return Result<bool>.Failure(MensajeError.ErrorEliminacion(nameof(Viaje)));

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Hubo un error al eliminar el viaje: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(string patente)
        {
            try
            {
                // Verificar que el camión existe usando el servicio
                var camionResult = await _camionService.ObtenerPorPatenteAsync(patente);
                if (camionResult.IsSuccess)
                {
                    int idCamion = camionResult.Value.Id;
                    var viajes = await _viajeRepository.ObtenerPorCamionAsync(idCamion, patente);

                    if (viajes == null)
                        return Result<List<ViajeDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");

                    return Result<List<ViajeDTO>>.Success(viajes);
                }
                return Result<List<ViajeDTO>>.Failure($"El camión especificado no existe: {camionResult}");
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por camión: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeMixtoDTO>>> ObtenerPorClienteAsync(int idCliente)
        {
            if (idCliente < 0)
                return Result<List<ViajeMixtoDTO>>.Failure(MensajeError.IdInvalido(idCliente));
            
            try
            {
                // Verificar que el cliente existe usando el servicio
                
                var clienteResult = await _clienteService.ObtenerPorIdAsync(idCliente);

                if (!clienteResult.IsSuccess)
                    return Result<List<ViajeMixtoDTO>>.Failure($"El cliente especificado no existe: {clienteResult}");
                
                var viajes = await _viajeRepository.ObtenerViajeMixtoPorClienteAsync(idCliente);

                if (viajes == null)
                    return Result<List<ViajeMixtoDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");

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
                return Result<List<ViajeDTO>>.Failure(MensajeError.IdInvalido(idChofer));
            }
            try
            {
                // Verificar que el chofer existe usando el servicio
                var choferResult = await _choferService.ObtenerPorIdAsync(idChofer);

                if (choferResult.Value == null)
                    return Result<List<ViajeDTO>>.Failure(MensajeError.EntidadNoEncontrada(nameof(choferResult.Value), idChofer));

                var viajes = await _viajeRepository.ObtenerPorChoferAsync(choferResult.Value.Nombre);

                if (viajes == null)
                    return Result<List<ViajeDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");

                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por chofer: {ex.Message}");
            }
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(string nombreChofer)
        {
            try
            {
                var viajes = await _viajeRepository.ObtenerPorChoferAsync(nombreChofer);

                if (viajes == null)
                    return Result<List<ViajeDTO>>.Failure("Ocurrió un error al obtener la lista de viajes");

                return Result<List<ViajeDTO>>.Success(viajes);
            }
            catch (Exception ex)
            {
                return Result<List<ViajeDTO>>.Failure($"Error al obtener viajes por nombre de chofer: {ex.Message}");
            }
        }

        public async Task<bool> TienePagoPendiente(int id)
        {
            var result = await _pagoService.ObtenerPorIdViajeAsync(id);

            if (result.IsSuccess)
                return !result.Value.Pagado;
            
            return false;
        }
    }
}
