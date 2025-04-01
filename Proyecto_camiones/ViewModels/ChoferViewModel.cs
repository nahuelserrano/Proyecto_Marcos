using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;

namespace Proyecto_camiones.ViewModels
{
    class ChoferViewModel
    {
        private ChoferService _choferService;

        public ChoferViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var choferRepository = new ChoferRepository(dbContext);
            this._choferService = new ChoferService(choferRepository);
        }

        public async Task<bool> testearConexion()
        {
            return await this._choferService.ProbarConexionAsync();
        }


        public async Task<ChoferDTO> ObtenerPorId(int id)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._choferService.ObtenerPorId(id);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    ChoferDTO chofer = resultado.Value;
                    Console.WriteLine($"Chofer encontrado con ID: {id}");
                    return chofer;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al obtener el chofer: {resultado.Error}");
                    return null;
                }
            }
            return null; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<int> Insertar(string nombre, string apellido)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._choferService.CrearChofer(nombre, apellido);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idChofer = resultado.Value;
                    Console.WriteLine($"Chofer creado con ID: {idChofer}");
                    return idChofer;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el chofer: {resultado.Error}");
                    return -1;
                }
            }
            return -1; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<bool> Eliminar(int id)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._choferService.EliminarChofer(id);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    Console.WriteLine($"Chofer eliminado con ID: {id}");
                    return true;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al eliminar el chofer: {resultado.Error}");
                    return false;
                }
            }
            return false; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<bool> Actualizar(int id, string nombre, string apellido)
        {
            if (this.testearConexion().Result)
            {
                var resultado = await this._choferService.EditarChofer(id, nombre, apellido);
                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    Console.WriteLine($"Chofer actualizado con ID: {id}");
                    return true;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al actualizar el chofer: {resultado.Error}");
                    return false;
                }
            }
            return false; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }

        public async Task<List<ChoferDTO>> ObtenerTodos()
        {
            if (this.testearConexion().Result)
            {
                var choferes = await this._choferService.ObtenerTodos();
                Console.WriteLine("no rompió ante la llamada");
                return choferes.Value;
            }
            return null; //provisorio, averiguar que tipo de respuesta da, un simil response entity de java
        }
    }
}
