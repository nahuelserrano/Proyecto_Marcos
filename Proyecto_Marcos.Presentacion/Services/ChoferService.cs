using System;
using System.Threading.Tasks;
using Proyecto_Marcos.Presentacion.models;
using TransporteApp.Utils;

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

}
