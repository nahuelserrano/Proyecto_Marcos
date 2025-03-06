using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Repositories;
using Proyecto_Marcos.Presentacion.Utils;

public class ChoferService
{
    private ChoferRepository _choferRepository;
	
	public ChoferService(ChoferRepository choferRepository)
    {
        this._choferRepository = choferRepository ?? throw new ArgumentNullException(nameof(choferRepository));
    }

    public async Task<Result<int>> getByIdAsync(int id)
    {
        if (id <= 0)
            return Result<int>.Failure(MensajeError.idInvalido(id));

        Chofer chofer = await this._choferRepository.getById(id);

        if (chofer == null)
            return Result<int>.Failure(MensajeError.objetoNulo(nameof(chofer)));
        
        return Result<int>.Success(id);
    }

    internal async Task<Result<bool>> eliminarCamionAsync(int choferId)
    {
        if (choferId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(choferId));

        Camion camion = await this._choferRepository.getById(choferId);

        if (camion == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(camion)));

        this._choferRepository.eliminarChofer(choferId);

        return Result<bool>.Success(true);
    }

    public async Task<Result<int>> CrearChoferAsync(Chofer chofer)
    {
        
        ValidadorChofer validador = new ValidadorChofer(chofer);
        Result<bool> resultadoValidacion = validador.ValidarCompleto();
        if (!resultadoValidacion.IsSuccess)
            return Result<int>.Failure(resultadoValidacion.Error);


        try
        {
            int idChofer = await _choferRepository.insertarChoferAsync(chofer);
            return Result<int>.Success(idChofer);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("Hubo un error al crear el chofer: " + ex.Message);
        }
    }

    public async Task<Result<int>> EditarChoferAsync(int id,string nombre = null,String apellido = null)
    {
        if (id <= 0)
            return Result<int>.Failure(MensajeError.idInvalido(id));

        var vehiculoExistente = await _choferRepository.ObtenerPorIdAsync(id);

        if (vehiculoExistente == null)
            return Result<int>.Failure(MensajeError.objetoNulo(nameof(vehiculoExistente)));

        if (!string.IsNullOrWhiteSpace(nombre))
            vehiculoExistente.Patente = nombre;

        if (!string.IsNullOrWhiteSpace(apellido))
            vehiculoExistente.Patente = apellido;

        await _choferRepository.ActualizarAsync(vehiculoExistente);

        return Result<int>.Success(id);
    }

}
