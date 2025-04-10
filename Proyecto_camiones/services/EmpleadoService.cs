using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.Presentacion.Services
{
    public class EmpleadoService
    {
        private EmpleadoRepository _empleadoRepository;
        private CamionService _camionService;

        public EmpleadoService(EmpleadoRepository empleadoRepository)
        {
            this._empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this._empleadoRepository.ProbarConexionAsync();
            return result;
        }

        public async Task<Result<EmpleadoDTO>> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return Result<EmpleadoDTO>.Failure(MensajeError.idInvalido(id));

            Empleado empleado = await this._empleadoRepository.ObtenerPorIdAsync(id);

            if (empleado == null)
                return Result<EmpleadoDTO>.Failure(MensajeError.objetoNulo(nameof(empleado)));

            return Result<EmpleadoDTO>.Success(empleado.toDTO());
        }

        public async Task<Result<bool>> EliminarEmpleadoAsync(int id)
        {
            if (id <= 0) return Result<bool>.Failure(MensajeError.idInvalido(id));

            Empleado empleado = await this._empleadoRepository.ObtenerPorIdAsync(id);

            if (empleado == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(empleado)));

            await this._empleadoRepository.EliminarEmpleadoAsyn(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<int>> CrearEmpleadoAsync(string nombre)
        {
            ValidadorEmpleado validador = new ValidadorEmpleado(nombre);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                int idEmpleado = await _empleadoRepository.InsertarEmpleadoAsync(nombre);
                return Result<int>.Success(idEmpleado);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el empleado: " + ex.Message);
            }
        }

        public async Task<Result<int>> EditarEmpleadoAsync(int id, string nombre = null)
        {
            if (id <= 0)
                return Result<int>.Failure(MensajeError.idInvalido(id));

            var empleado = await _empleadoRepository.ObtenerPorIdAsync(id);

            if (empleado == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(empleado)));

            if (!string.IsNullOrWhiteSpace(nombre))
                empleado.nombre = nombre;

            await _empleadoRepository.ActualizarEmpleadoAsync(id, nombre);

            return Result<int>.Success(id);
        }

        public async Task<Result<List<EmpleadoDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var empleados = await _empleadoRepository.ObtenerEmpleadosAsync();

                if (empleados == null)
                    return Result<List<EmpleadoDTO>>.Failure(MensajeError.objetoNulo(nameof(empleados)));

                List<EmpleadoDTO> empleadosDTO = new List<EmpleadoDTO>();

                foreach (var empleado in empleados)
                {
                    empleadosDTO.Add(empleado.toDTO());
                }

                return Result<List<EmpleadoDTO>>.Success(empleadosDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
