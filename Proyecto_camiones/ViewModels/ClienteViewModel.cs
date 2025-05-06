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
using System.Windows.Forms;

namespace Proyecto_camiones.ViewModels
{
    public class ClienteViewModel
    {

        private readonly ClienteService _clienteService;

        public ClienteViewModel()
        {
            MessageBox.Show("ClienteViewModel");
            var dbContext = General.obtenerInstancia();
            var clienteRepo = new ClienteRepository(dbContext);
            this._clienteService = new ClienteService(clienteRepo);
        }

        public async Task<bool> testearConexion()
        {
            return await this._clienteService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarCliente(string nombre)
        {
            if (await this.testearConexion())
            {
                var resultado = await this._clienteService.InsertarAsync(nombre.ToUpper());

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
            if (await this.testearConexion())
                return await this._clienteService.ObtenerPorIdAsync(id);

            return Result<Cliente>.Failure("No se pudo establecer la conexion");
        }

        public async Task<Result<bool>> Eliminar(int id)
        {
            if (await this.testearConexion())
            {
                return await this._clienteService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión");
        }

        public async Task<Result<List<ViajeMixtoDTO>>> ObtenerViajesDeUnCliente(string cliente)
        {
            MessageBox.Show("ObtenerViajesDeUnCliente");
            if (await this.testearConexion())
            {
                MessageBox.Show("Testear conexión");
                return await this._clienteService.ObtenerViajesDeUnClienteAsync(cliente.ToUpper());
            }
            return Result<List<ViajeMixtoDTO>>.Failure("No se pudo establecer la conexión");
        }

        public async Task<Result<List<Cliente>>> ObtenerTodosAsync()
        {
            bool conexion = await this.testearConexion();
            if (conexion)
            {
                return await this._clienteService.ObtenerTodosAsync();
            }
            return Result<List<Cliente>>.Failure("No se pudo establecer la conexión");
        }

    }
}