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
        private readonly ViajeFleteService _viajeService;

        public ViajeViewModel()
        {
            // Obtenemos la instancia de la base de datos
            var dbContext = General.obtenerInstancia();

            // Creamos las dependencias necesarias
            var viajeRepository = new ViajeRepository(dbContext);
            var camionRepository = new CamionRepository(dbContext);
            var empleadoRepository = new EmpleadoRepository(dbContext);

            // Creamos los servicios que ViajeService necesita
            var camionService = new CamionService(camionRepository);
            var empleadoService = new EmpleadoService(empleadoRepository);

            // Finalmente creamos el servicio de viajes con todas sus dependencias
            this._viajeService = new ViajeFleteService(viajeRepository, camionService, empleadoService);
        }

        // Método para probar la conexión
        public async Task<bool> TestearConexion()
        {
            // Asumimos que ViajeService tiene un método similar
            return await this._viajeService.ProbarConexionAsync();
        }

        // Método para crear un nuevo viaje
        public async Task<Result<ViajeDTO>> CrearViaje(
            string destino,
            string lugarPartida,
            float kg,
            int remito,
            float precioPorKilo,
            int chofer,
            int cliente,
            int camion,
            DateOnly fechaInicio,
            DateOnly fechaFacturacion,
            string carga,
            float km)
        {
            if (await this.TestearConexion())
            {
                var resultado = await this._viajeService.CrearViajeAsync(
                    destino, lugarPartida, kg, remito, precioPorKilo,
                    chofer, cliente, camion, fechaInicio, carga, km, fechaFacturacion);

                if (resultado.IsSuccess)
                {
                    Console.WriteLine($"Viaje creado con ID: {resultado.Value}");
                    return resultado;
                }
                else
                {
                    Console.WriteLine($"Error al crear el viaje: {resultado.Error}");
                    return Result<ViajeDTO>.Failure(resultado.Error);
                }
            }
            return Result<ViajeDTO>.Failure("La conexión no pudo establecerse");
        }

        // Método para obtener todos los viajes
        public async Task<Result<List<ViajeDTO>>> ObtenerTodos()
        {
            if (await this.TestearConexion())
            {
                var resultado = await this._viajeService.ObtenerViajesAsync();

                if (resultado.IsSuccess)
                {
                    return resultado;
                }
                else
                {
                    return Result<List<ViajeDTO>>.Failure(resultado.Error);
                }
            }
            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        // Método para obtener viajes filtrados
        public async Task<Result<List<ViajeDTO>>> ObtenerFiltrados(
            DateOnly? fechaInicio = null,
            DateOnly? fechaFin = null,
            int? choferId = null,
            int? camionId = null)
        {
            if (await this.TestearConexion())
            {
                var resultado = await this._viajeService.ObtenerViajesAsync(
                    fechaInicio, fechaFin, choferId, camionId);

                if (resultado.IsSuccess)
                {
                    return resultado;
                }
                else
                {
                    return Result<List<ViajeDTO>>.Failure(resultado.Error);
                }
            }
            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        // Método para actualizar un viaje
        public async Task<Result<bool>> ActualizarViaje(
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
            if (await this.TestearConexion())
            {
                var resultado = await this._viajeService.EditarViajeAsync(
                    id, destino, lugarPartida, kg, remito, precioPorKilo,
                    empleado, cliente, camion, fechaInicio, fechaFacturacion,
                    carga, km);

                return resultado;
            }
            return Result<bool>.Failure("La conexión no pudo establecerse");
        }

        // Método para eliminar un viaje
        public async Task<Result<string>> EliminarViaje(int id)
        {
            if (await this.TestearConexion())
            {
                var resultado = await this._viajeService.EliminarViajeAsync(id);

                if (resultado.IsSuccess)
                {
                    return Result<string>.Success($"El viaje con ID {id} fue eliminado correctamente");
                }
                else
                {
                    return Result<string>.Failure($"El viaje con ID {id} no pudo ser eliminado: {resultado.Error}");
                }
            }
            return Result<string>.Failure("Error de conexión");
        }

        // Método para obtener viajes por camión
        public async Task<Result<List<ViajeDTO>>> ObtenerViajesPorCamion(int camionId)
        {
            if (await this.TestearConexion())
            {
                return await this._viajeService.ObtenerViajesPorCamionAsync(camionId);
            }
            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }

        // Método para obtener viajes por empleado/chofer
        public async Task<Result<List<ViajeDTO>>> ObtenerViajesPorEmpleado(int empleadoId)
        {
            if (await this.TestearConexion())
            {
                return await this._viajeService.ObtenerViajesPorEmpleado(empleadoId);
            }
            return Result<List<ViajeDTO>>.Failure("La conexión no pudo establecerse");
        }
    }
}