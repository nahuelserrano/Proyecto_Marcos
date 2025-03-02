using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Utils;


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

    public async Task<Result<int>> CrearcamionAsync(Camion camion)
    {
        if (camion.CapacidadMax == null || camion.Tara == null || camion.Patente == null) return Result<int>.Failure("¡datos incompletos!");

        if (camion.Tara < this.pesominimo) return Result<int>.Failure("la tara del camion no es correcta");

        if (camion.CapacidadMax < this.pesominimo) return Result<int>.Failure("el peso maximo del camion no es correcto");

        try
        {
            // Intentar insertar en la base de datos
            int idcamion = await _camionRepository.insertarcamionAsync(camion);
            return Result<int>.Success(idcamion);
        }
        catch (Exception ex)
        {
            // Si algo sale mal, registrar el error y devolver un mensaje amigable
            //Logger.LogError($"Error al crear camion: {ex.Message}"); 
            return Result<int>.Failure("Hubo un error al crear el camion");
        }
    }

    public async Task<Result<int>> EditarCamionAsync(int id, float? capacidadMax = null, float? tara = null, string patente = null)
    {
        if (id <= 0)
            return Result<int>.Failure("ID de vehículo inválido.");

        var vehiculoExistente = await _camionRepository.ObtenerPorIdAsync(id);

        if (vehiculoExistente == null)
            return Result<int>.Failure("El vehículo no existe.");
    
        if (capacidadMax.HasValue)
            vehiculoExistente.CapacidadMax = capacidadMax.Value;

        if (tara.HasValue)
            vehiculoExistente.Tara = tara.Value;

        if (!string.IsNullOrWhiteSpace(patente))
            vehiculoExistente.Patente = patente;

        await _camionRepository.ActualizarAsync(vehiculoExistente);

        return Result<int>.Success(id);
    }
}
