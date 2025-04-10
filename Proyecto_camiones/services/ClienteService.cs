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


        //public async Task<Result<bool>> Eliminar(int clienteId)
        //{
        //    if (clienteId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(clienteId));

        //    // int idCamion = await this._clienteRepository.ObtenerIdCamion(clienteId); A implmentar en el repositorio

        //    int idCamion = 1; // Simulamos el id del camion

        //    //Result<Camion> camion = await this._camionService.ObtenerPorId(idCamion); // error await

        //    //if (camion.Value == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(camion)));

        //    await this._clienteRepository.Eliminar(clienteId);

        //    return Result<bool>.Success(true);

        //}

        //public async Task<Result<int>> Crear(String nombre, String apellido, string dni)
        //{
        //    ValidadorCliente validador = new ValidadorCliente(nombre, apellido, dni);
        //    Result<bool> resultadoValidacion = validador.ValidarCompleto();

        //    if (!resultadoValidacion.IsSuccess)
        //        return Result<int>.Failure(resultadoValidacion.Error);

        //    try
        //    {
        //        int idCliente = await _clienteRepository.Insertar(nombre, apellido, dni);  
        //        return Result<int>.Success(idCliente);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
        //    }
        //}



        //public async Task<Result<int>> Actualizar(int id, String nombre, String apellido, string dni)
        //{
        //   //lo dejo en para que lo chequeen, si mandamos el id para corregir a la funcion del
        //   //validador tambien nos hace mandarlo en crear. por eso propongo dejar el chequeo aca
        //    if(id <= 0)
        //        return Result<int>.Failure(MensajeError.idInvalido(id));

        //    ValidadorCliente validador = new ValidadorCliente( nombre, apellido, dni);
        //    Result<bool> resultadoValidacion = validador.ValidarCompleto();
            
        //    if (!resultadoValidacion.IsSuccess)
        //        return Result<int>.Failure(resultadoValidacion.Error);


        //    var clienteExistente = await _clienteRepository.ObtenerPorId(id);

        //    if (clienteExistente == null)
        //        return Result<int>.Failure(MensajeError.objetoNulo(nameof(clienteExistente)));



        //    if (!string.IsNullOrWhiteSpace(dni))
        //        clienteExistente.Dni = dni;

        //    if (!string.IsNullOrWhiteSpace(nombre))
        //        clienteExistente.Nombre = nombre;


        //    if (!string.IsNullOrWhiteSpace(apellido))
        //        clienteExistente.Apellido = apellido;

        //    await _clienteRepository.Actualizar(id, nombre, apellido, dni);

        //    return Result<int>.Success(id);
        //}

    }
}
