using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;

namespace Proyecto_camiones.ViewModels
{
    public class ViajeViewModel
    {
        private readonly ViajeService _viajeService;

        public ViajeViewModel()
        { 
            // Obtenemos la instancia de la base de datos
            // Creamos las dependencias necesarias
            var viajeRepository = new ViajeRepository();
            var camionRepository = new CamionRepository();
            var clienteRepository = new ClienteRepository();
            var choferRepository = new ChoferRepository();
            var pagoRepository = new PagoRepository();

            // Creamos los servicios que ViajeService necesita
            var camionService = new CamionService(camionRepository);
            var clienteService = new ClienteService(clienteRepository);
            var choferService = new ChoferService(choferRepository);
            var pagoService = new PagoService(pagoRepository);


            // Finalmente creamos el servicio de viajes con todas sus dependencias
            _viajeService = new ViajeService(viajeRepository, camionService, clienteService, choferService, pagoService);
        }

        // Método para probar la conexión
        public async Task<bool> TestearConexion()
        {
            return await _viajeService.ProbarConexionAsync();
        }

        // Método para crear un nuevo viaje
        public async Task<Result<int>> CrearAsync(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            string carga,
            float kg,
            string cliente,
            string camion,
            float km,
            float tarifa,
            string nombreChofer,
            float porcentajeChofer)
        {
            bool conexionExitosa = await TestearConexion();

            Console.WriteLine(await TestearConexion());

            if (!conexionExitosa)
            {
                return Result<int>.Failure(MensajeError.errorConexion());
            }

            var resultado = await _viajeService.CrearAsync(
                fechaInicio, lugarPartida, destino, remito, kg, carga, 
                cliente, camion, km, tarifa, nombreChofer, porcentajeChofer);
            if (resultado.IsSuccess)
            {
                Console.WriteLine($"Viaje creado con éxito");
                return resultado;
            }

            Console.WriteLine($"Error al crear el viaje: {resultado.Error}");
            return Result<int>.Failure(resultado.Error);
        }

        // Método para obtener un viaje por ID
        public async Task<Result<ViajeDTO>> ObtenerPorIdAsync(int id)
        {
            if (await this.TestearConexion())
            {
                var resultado = await _viajeService.ObtenerPorIdAsync(id);
                if (resultado.IsSuccess)
                {
                    return resultado;
                }

                return Result<ViajeDTO>.Failure(resultado.Error);

            }

            return Result<ViajeDTO>.Failure("La conexión no pudo establecerse");
        }

        // Método para obtener todos los viajes
        public async Task<Result<List<ViajeDTO>>> ObtenerTodosAsync()
        {
            if (await this.TestearConexion())
            {
                var resultado = await _viajeService.ObtenerTodosAsync();

                if (resultado.IsSuccess)
                {
                    return resultado;
                }

                return Result<List<ViajeDTO>>.Failure(resultado.Error);

            }

            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        //        // Método para obtener viajes filtrados
        public async Task<Result<List<ViajeDTO>>> ObtenerPorFiltradoAsync(
            DateOnly? fechaInicio = null,
            DateOnly? fechaFin = null,
            int? choferId = null,
            int? camionId = null)
        {
            if (await this.TestearConexion())
            {
                // Implementar lógica de filtrado usando los repositorios adecuados
                var viajes = await _viajeService.ObtenerTodosAsync();

                if (!viajes.IsSuccess)
                {
                    return Result<List<ViajeDTO>>.Failure(viajes.Error);
                }

                // Aquí iría la lógica de filtrado
                // Por ahora, solo devolvemos todos los viajes
                return viajes;
            }

            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        // Método para actualizar un viaje
        public async Task<Result<bool>> ActualizarAsync(
            int id,
            DateOnly? fechaInicio = null,
            string? lugarPartida = null,
            string? destino = null,
            int? remito = null,
            string carga = null,
            float? kg = null,
            string? nombreCliente = null,
            string? patenteCamion = null,
            float? km = null,
            float? tarifa = null,
            string? nombreChofer = null,
            float? porcentaje = null)
        {
            if (await this.TestearConexion())
            {
                var resultado = await _viajeService.ActualizarAsync(
                    id, fechaInicio, lugarPartida, destino, remito, kg, carga, 
                    nombreCliente, patenteCamion, km, tarifa, nombreChofer, porcentaje);

                return resultado;
            }

            return Result<bool>.Failure("La conexión no pudo establecerse");
        }

        // Método para eliminar un viaje
        public async Task<Result<string>> EliminarAsync(int id)
        {
            if (await this.TestearConexion())
            {
                var resultado = await _viajeService.EliminarAsync(id);

                if (resultado.IsSuccess)
                {
                    return Result<string>.Success($"El viaje con ID {id} fue eliminado correctamente");
                }
                else
                {
                    return Result<string>.Failure(resultado.Error);
                }
            }

            return Result<string>.Failure("Error de conexión");
        }

        // Método para obtener viajes por camión
        public async Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(string patenteCamion)
        {
            if (await this.TestearConexion())
            {
                return await _viajeService.ObtenerPorCamionAsync(patenteCamion);
            }

            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(int choferId)
        {
            if (await this.TestearConexion())
            {
                return await _viajeService.ObtenerPorChoferAsync(choferId);
            }

            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        public async Task<Result<List<ViajeMixtoDTO>>> ObtenerPorClienteAsync(int clienteId)
        {
            if (await this.TestearConexion())
                return await _viajeService.ObtenerPorClienteAsync(clienteId);

            return Result<List<ViajeMixtoDTO>>.Failure("La conexión no pudo establecerse");
        }

        public async Task<Result<List<ViajeDTO>>> ObtenerPorChoferAsync(string nombreChofer)
        {
            if (await this.TestearConexion())
            {
                return await _viajeService.ObtenerPorChoferAsync(nombreChofer);
            }

            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }
    }
}