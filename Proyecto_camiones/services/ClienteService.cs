using System;
using System.Threading.Tasks;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Utils;


namespace Proyecto_camiones.Presentacion.Services
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
                return Result<int>.Failure(MensajeError.idInvalido(id));

            Cliente cliente = await this._clienteRepository.ObtenerPorId(id);

            if (cliente == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(cliente)));

            return Result<int>.Success(id);
        }


        public async Task<Result<bool>> Eliminar(int clienteId)
        {
            if (clienteId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(clienteId));

            // int idCamion = await this._clienteRepository.ObtenerIdCamion(clienteId); A implmentar en el repositorio

            int idCamion = 1; // Simulamos el id del camion

            Result<Camion> camion = await this._camionService.ObtenerPorId(idCamion); // error await

            if (camion.Value == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(camion)));

            await this._clienteRepository.Eliminar(clienteId);

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
                return Result<int>.Failure(MensajeError.idInvalido(id));


            var clienteExistente = await _clienteRepository.ObtenerPorId(id);

            if (clienteExistente == null)
                return Result<int>.Failure(MensajeError.objetoNulo(nameof(clienteExistente)));




            if (!string.IsNullOrWhiteSpace(nombre))
                clienteExistente.nombre = nombre;


            if (!string.IsNullOrWhiteSpace(apellido))
                clienteExistente.apellido = apellido;

            await _clienteRepository.Actualizar(clienteExistente);

            return Result<int>.Success(id);
        }

    }
}
