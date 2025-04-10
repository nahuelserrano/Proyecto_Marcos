using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;

namespace Proyecto_camiones.ViewModels
{
    public class EmpleadoViewModel
    {
        private EmpleadoService _empleadoService;

        public EmpleadoViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var empleadoRepository = new EmpleadoRepository(dbContext);
            this._empleadoService = new EmpleadoService(empleadoRepository);
        }

        public async Task<bool> testearConexion()
        {
            return await this._empleadoService.ProbarConexionAsync();
        }

        public async Task<Result<EmpleadoDTO>> ObtenerPorId(int id)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._empleadoService.ObtenerPorIdAsync(id);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    EmpleadoDTO empleado = resultado.Value;
                    Console.WriteLine($"Empleado encontrado con ID: {id}");
                    return Result<EmpleadoDTO>.Success(empleado);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al obtener el empleado: {resultado.Error}");
                    return Result<EmpleadoDTO>.Failure($"Error al obtener el empleado: {resultado.Error}");
                }
            }
            return null; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<Result<int>> InsertarEmpleado(string nombre)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._empleadoService.CrearEmpleadoAsync(nombre);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idEmpleado = resultado.Value;
                    Console.WriteLine($"Empleado creado con ID: {idEmpleado}");
                    return Result<int>.Success(idEmpleado);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el empleado: {resultado.Error}");
                    return Result<int>.Failure($"Error al crear el empleado: {resultado.Error}");
                }
            }
            return Result<int>.Failure("Error de conexión"); //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<Result<bool>> Eliminar(int id)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._empleadoService.EliminarEmpleadoAsync(id);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    Console.WriteLine($"Empleado eliminado con ID: {id}");
                    return Result<bool>.Success(true);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al eliminar el empleado: {resultado.Error}");
                    return Result<bool>.Failure($"Error al crear el empleado: {resultado.Error}");
                }
            }
            return Result<bool>.Failure("Error de conexión"); //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<Result<bool>> Actualizar(int id, string nombre)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._empleadoService.EditarEmpleadoAsync(id, nombre);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    Console.WriteLine($"Empleado actualizado con ID: {id}");
                    return Result<bool>.Success(true);
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al actualizar el empleado: {resultado.Error}");
                    return Result<bool>.Failure($"Error al actualizar el empleado: {resultado.Error}");
                }
            }
            return Result<bool>.Failure(""); //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<Result<List<EmpleadoDTO>>> ObtenerTodos()
        {
            if (this.testearConexion().Result)
            {
                var empleados = await this._empleadoService.ObtenerTodosAsync();
                Console.WriteLine("no rompió ante la llamada");
                return Result<List<EmpleadoDTO>>.Success(empleados.Value);
            }
            return null; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }
    }
}
