using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class ViajeViewModel
    {
        private readonly ViajeService _viajeService;

        public ViajeViewModel()
        {
            // Obtenemos la instancia de la base de datos
            var dbContext = General.obtenerInstancia();

            // Creamos las dependencias necesarias
            var viajeRepository = new ViajeRepository(dbContext);
            var camionRepository = new CamionRepository(dbContext);
            var clienteRepository = new ClienteRepository(dbContext);
            var choferRepository = new ChoferRepository(dbContext);

            // Creamos los servicios que ViajeService necesita
            var camionService = new CamionService(camionRepository);
            var clienteService = new ClienteService(clienteRepository);
            var choferService = new ChoferService(choferRepository);


            // Finalmente creamos el servicio de viajes con todas sus dependencias
            _viajeService = new ViajeService(viajeRepository, camionService, clienteService, choferService);
        }

        // Método para probar la conexión
        public async Task<bool> TestearConexion()
        {
            return await _viajeService.ProbarConexionAsync();
        }

        // Método para crear un nuevo viaje
        public async Task<Result<ViajeDTO>> CrearAsync(
            DateOnly fechaInicio,
            string lugarPartida,
            string destino,
            int remito,
            string carga,
            float kg,
            int cliente,
            int camion,
            float km,
            float tarifa)
        {
            Console.WriteLine(4);

            bool conexionExitosa = await TestearConexion();

            Console.WriteLine(await TestearConexion());

            if (!conexionExitosa)
            {
                return Result<ViajeDTO>.Failure(MensajeError.errorConexion());
            }

            var resultado = await _viajeService.CrearAsync(
                fechaInicio, lugarPartida, destino, remito,
                kg, carga, cliente, camion, km, tarifa);

            if (resultado.IsSuccess)
            {
                Console.WriteLine($"Viaje creado con éxito");
                return resultado;
            }
            
            Console.WriteLine($"Error al crear el viaje: {resultado.Error}");
            return Result<ViajeDTO>.Failure(resultado.Error);
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

        // Método para obtener viajes filtrados
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
            string lugarPartida = null,
            string destino = null,
            int? remito = null,
            string carga = null,
            float? kg = null,
            int? cliente = null,
            int? camion = null,
            float? km = null,
            float? tarifa = null)
        {
            if (await this.TestearConexion())
            {
                var resultado = await _viajeService.ActualizarAsync(
                    id, fechaInicio, lugarPartida, destino, remito,
                    kg, carga, cliente, camion, km, tarifa);

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
        public async Task<Result<List<ViajeDTO>>> ObtenerPorCamionAsync(int camionId)
        {
            if (await this.TestearConexion())
            {
                return await _viajeService.ObtenerPorCamionAsync(camionId);
            }
            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }
    }
}