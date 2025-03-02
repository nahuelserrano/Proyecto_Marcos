using System;
using System.Threading.Tasks;
using TransporteApp.Utils;
using Proyecto_Marcos.Presentacion.models;
using TransporteApp.Repositories;


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
            return Result<int>.Failure("El id no puede ser menor a 0");

        Chofer chofer = await this._choferRepository.getById(id);

        if (chofer == null)
            return Result<int>.Failure("El chofer con el id " + id + " No existe");
        
        return Result<int>.Success(id);
    }
    internal async Task<Result<bool>> eliminarCamionAsync(int choferId)
    {
        if (choferId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

        Camion camion = await this._choferRepository.getById(choferId);

        if (camion == null) return Result<bool>.Failure("El camion con el id " + choferId + " No existe");

        this._choferRepository.eliminarChofer(choferId);

        return Result<bool>.Success(true);

    }

    public async Task<Result<int>> CrearChoferAsync(Chofer chofer)
    {
        if (chofer == null)
            return Result<int>.Failure("El objeto chofer no puede ser nulo");

        
        if (string.IsNullOrWhiteSpace(chofer.nombre) || string.IsNullOrWhiteSpace(chofer.apellido))
            return Result<int>.Failure("¡Datos incompletos! El nombre y el apellido son obligatorios.");

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
            return Result<int>.Failure("ID de chofer inválido.");


        var vehiculoExistente = await _choferRepository.ObtenerPorIdAsync(id);

        if (vehiculoExistente == null)
            return Result<int>.Failure("El vehículo no existe.");


        

        if (!string.IsNullOrWhiteSpace(nombre))
            vehiculoExistente.Patente = nombre;


        if (!string.IsNullOrWhiteSpace(apellido))
            vehiculoExistente.Patente = apellido;

        await _choferRepository.ActualizarAsync(vehiculoExistente);

        return Result<int>.Success(id);
    }

}
