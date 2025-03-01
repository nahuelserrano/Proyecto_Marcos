using System;
using System.Threading.Tasks;
using TransporteApp.Utils;
using Proyecto_Marcos.Presentacion.models;
using TransporteApp.Repositories;


public class CamionService
{
    private CamionRepository _camionRepository;
    private int pesominimo = 1;

    public CamionService(CamionRepository camionR)
    {
        this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
    }

    public async Task<Result<Camion>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<Camion>.Failure("El id no puede ser menor a 0");

        Camion camion = await this._camionRepository.getById(id);

        if (camion == null)
            return Result<Camion>.Failure("El camion con el id " + id + " No existe");
      
        return Result<Camion>.Success(camion);
    }

    internal async Task<Result<bool>> eliminarCamionAsync(int camionId)
    {
        if (camionId <= 0) return Result<bool>.Failure("El id no puede ser menor a 0");

        Camion camion = await this._camionRepository.getById(camionId);
       
        if (camion == null) return Result<bool>.Failure("El camion con el id " + camionId + " No existe");

        this._camionRepository.eliminarCamion(camionId);
       
        return Result<bool>.Success(true);

    }


    public async Task<Result<int>> CrearcamionAsync(float capacidadMax, float tara, String patente)
    {
        if (capacidadMax == null|| tara == null|| patente == null) return Result<int>.Failure("¡datos incompletos!");



        if ( tara < this.pesominimo) return Result<int>.Failure("la tara del camion no es correcta");
        if (capacidadMax < this.pesominimo) return Result<int>.Failure("el peso maximo del camion no es correcto");



        // Validar el peso de la carga contra la capacidad del camión
        Camion camion = await _camionService.ObtenerCamionAsync(camion.CamionId);
        if (camion.KilosCarga > camion.CapacidadMaxima)
            return Result<int>.Failure($"¡La carga excede la capacidad! Máximo permitido: {camion.CapacidadMaxima}kg");


        try
        {
            // Intentar insertar en la base de datos
            int idcamion = await _camionRepository.InsertarcamionAsync(camion);
            return Result<int>.Success(idcamion);
        }
        catch (Exception ex)
        {
            // Si algo sale mal, registrar el error y devolver un mensaje amigable
            //Logger.LogError($"Error al crear camion: {ex.Message}"); 
            return Result<int>.Failure("Hubo un error al crear el camion");
        }
    }
}
