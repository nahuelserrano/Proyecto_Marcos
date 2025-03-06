using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.Models;
using Proyecto_Marcos.Presentacion.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


public class CamionService
{
    private CamionRepository _camionRepository;
  

    public CamionService(CamionRepository camionR)
    {
        this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
    }

    public async Task<Result<Camion>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<Camion>.Failure(MensajeError.idInvalido(id));

        Camion camion = await this._camionRepository.getById(id);

        if (camion == null)
            return Result<Camion>.Failure(MensajeError.objetoNulo(nameof(camion)));
      
        return Result<Camion>.Success(camion);
    }

    internal async Task<Result<bool>> eliminarCamionAsync(int camionId)
    {
        if (camionId <= 0) return Result<bool>.Failure(MensajeError.idInvalido(camionId));

        Camion camion = await this._camionRepository.getById(camionId);
       
        if (camion == null) return Result<bool>.Failure(MensajeError.objetoNulo(nameof(camion)));

        this._camionRepository.eliminarCamion(camionId);
       
        return Result<bool>.Success(true);

    }

    public async Task<Result<int>> CrearcamionAsync(Camion camion)
    {


        ValidadorCamion validador = new ValidadorCamion(camion);
      
        Result<bool> resultadoValidacion = validador.ValidarCompleto();

        if (!resultadoValidacion.IsSuccess)
            return Result<int>.Failure(resultadoValidacion.Error);
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
            return Result<int>.Failure(MensajeError.objetoNulo(nameof(vehiculoExistente)));
    
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
