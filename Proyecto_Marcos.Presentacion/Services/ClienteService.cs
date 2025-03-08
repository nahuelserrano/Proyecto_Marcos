using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Utils;


namespace Proyecto_Marcos.Presentacion.Services
{
    class ClienteService
    {
        private ClienteRepository _clienteRepository;
        private CamionService _camionService;

        public ClienteService(ClienteRepository clienteRepository)
        {
            this._clienteRepository = _clienteRepository ?? throw new ArgumentNullException(nameof(_clienteRepository));
        }

        public async Task<Result<int>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<int>.Failure("El id no puede ser menor a 0");

            Cliente cliente = await this._clienteRepository.ObtenerPorId(id);

            if (cliente == null)
                return Result<int>.Failure("El chofer con el id " + id + " No existe");

            return Result<int>.Success(id);
        }


        public async Task<Result<bool>> Eliminar(int clienteId)
        {
            if (clienteId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

            // int idCamion = await this._clienteRepository.ObtenerIdCamion(clienteId); A implmentar en el repositorio

            int idCamion = 1; // Simulamos el id del camion

            Camion camion = await this._camionService.ObtenerPorId(idCamion).Result; // error await

            if (camion == null) return Result<bool>.Failure("El camion con el id " + clienteId + " No existe");

            this._clienteRepository.Eliminar(clienteId);

            return Result<bool>.Success(true);

        }

        public async Task<Result<int>> Crear(Cliente cliente)
        {
            ValidadorCliente validador = new ValidadorCliente(cliente);
            Result<bool> resultadoValidacion = validador.ValidarCompleto();

            if (!resultadoValidacion.IsSuccess)
                return Result<int>.Failure(resultadoValidacion.Error);

            try
            {
                int idCliente = await _clienteRepository.Insertar(cliente);  
                return Result<int>.Success(idCliente);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
            }
        }



        public async Task<Result<int>> Actualizar(int id,Camion viaje, String nombre, String apellido, int dni)
        {
            if (id <= 0)
                return Result<int>.Failure("ID de chofer inválido.");


            var clienteExistente = await _clienteRepository.ObtenerPorId(id);

            if (clienteExistente == null)
                return Result<int>.Failure("El vehículo no existe.");




            if (!string.IsNullOrWhiteSpace(nombre))
                clienteExistente.Patente = nombre;


            if (!string.IsNullOrWhiteSpace(apellido))
                clienteExistente.Patente = apellido;

            await _clienteRepository.Actualizar(clienteExistente);

            return Result<int>.Success(id);
        }

    }
}
