using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class ClienteViewModel
    {

        private ClienteService clienteService;

        public ClienteViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var clienteRepo = new ClienteRepository(dbContext);
            this.clienteService = new ClienteService(clienteRepo);
        }

        public async Task<bool> testearConexion()
        {
            return await this.clienteService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarCliente(string nombre)
        {
            if (this.testearConexion().Result)
            {
                Console.WriteLine("omg entré!!");
                var resultado = await this.clienteService.InsertarAsync(nombre);

                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idCliente = resultado.Value;
                    Console.WriteLine($"Cliente creado con ID: {idCliente}");
                    return resultado;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    Console.WriteLine($"Error al crear el camión: {resultado.Error}");
                    return Result<int>.Failure(resultado.Error);
                }
            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }

        public async Task<Result<Cliente>> ObtenerById(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.clienteService.ObtenerByIdAsync(id);
            }
            return Result<Cliente>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<bool>> Eliminar(int id)
        {
            if (this.testearConexion().Result)
            {
                return await this.clienteService.Eliminar(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión");
        } 

        public async Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnCliente(string cliente)
        {
            if (this.testearConexion().Result)
            {
                return await this.clienteService.ObtenerViajesDeUnCliente(cliente.ToUpper());
            }
            return Result<List<ViajeMixtoDTO>>.Failure("No se pudo establecer la conexión");
        }
    }
}
