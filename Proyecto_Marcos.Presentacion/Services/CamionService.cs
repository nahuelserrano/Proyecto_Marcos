using System;
using static ChoferService;
using System.Threading.Tasks;
using TransporteApp.Utils;
using TransporteApp.Models;


public class CamionService
{
    private CamionRepository _camionRepository;
    
    public CamionService(CamionRepository camionR)
    {
        this._camionRepository = camionR ?? throw new ArgumentNullException(nameof(camionR));
    }

    public async Task<Result<int>> getByIdAsync(int id)
    {
        if (id <= 0)
            return Result<int>.Failure("El id no puede ser menor a 0");

        Camion camion = await this._camionRepository.getById(id);

        if (camion == null)
            return Result<int>.Failure("El camion con el id " + id + " No existe");

        return Result<int>.Success(id);
    }

    internal async Task<Camion> ObtenerCamionAsync(object camionId)
    {
        throw new NotImplementedException();
    }
}
