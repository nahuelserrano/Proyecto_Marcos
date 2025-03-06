using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Utils;


namespace Proyecto_Marcos.Presentacion.Services
{
    class ClienteService
    {
        private ClienteRepository _clienteRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            this._clienteRepository = _clienteRepository ?? throw new ArgumentNullException(nameof(_clienteRepository));
        }

        public async Task<Result<int>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<int>.Failure("El id no puede ser menor a 0");

            Cliente cliente = await this._clienteRepository.getById(id);

            if (cliente == null)
                return Result<int>.Failure("El chofer con el id " + id + " No existe");

            return Result<int>.Success(id);
        }


        internal async Task<Result<bool>> EliminarClienteAsync(int clienteId)
        {
            if (clienteId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

            Camion camion = await this._clienteRepository.getById(clienteId);

            if (camion == null) return Result<bool>.Failure("El camion con el id " + clienteId + " No existe");

            this._clienteRepository.eliminarChofer(clienteId);

            return Result<bool>.Success(true);

        }

        public async Task<Result<int>> CrearClienteAsync(Cliente cliente)
        {

            ValidadorCliente validador = new ValidadorCliente(cliente);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();
            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);
            {

                int idCliente = await _clienteRepository.insertarChoferAsync(cliente);
                return Result<int>.Success(idCliente);
            }
            catch (Exception ex)
            {

                return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
            }
        }



        public async Task<Result<int>> EditarClienteAsync(int id,Camion viaje, String nombre, String apellido, int dni)
        {
            if (id <= 0)
                return Result<int>.Failure("ID de chofer inválido.");


            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(id);

            if (clienteExistente == null)
                return Result<int>.Failure("El vehículo no existe.");




            if (!string.IsNullOrWhiteSpace(nombre))
                clienteExistente.Patente = nombre;


            if (!string.IsNullOrWhiteSpace(apellido))
                clienteExistente.Patente = apellido;

            await _clienteRepository.ActualizarAsync(clienteExistente);

            return Result<int>.Success(id);
        }

    }
}
