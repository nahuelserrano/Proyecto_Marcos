using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Services
{
    public class FleteService
    {

        public readonly FleteRepository fleteRepository;

        public FleteService(FleteRepository fleteRepository)
        {
            this.fleteRepository = fleteRepository ?? throw new ArgumentNullException(nameof(fleteRepository));
        }

        public async Task<bool> TestearConexion()
        {
            return await this.fleteRepository.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarFletero(string nombre)
        {
            if(nombre!= null)
            {
                nombre = nombre.ToUpper();
                int id = await this.fleteRepository.InsertarAsync(nombre);
                if (id > -1)
                {
                    return Result<int>.Success(id);
                }
                return Result<int>.Failure(MensajeError.ErrorCreacion("fletero"));
            }
            return Result<int>.Failure("El campo nombre no puede ser nulo");
        }

        internal async Task<Result<Flete>> ObtenerPorNombreAsync(string nombre)
        {
            if(nombre != null)
            {
                nombre = nombre.ToUpper();
                Flete fletero = await this.fleteRepository.ObtenerPorNombreAsync(nombre);
                if(fletero != null)
                {
                    return Result<Flete>.Success(fletero);
                }
                return Result<Flete>.Failure("No existe un fletero con ese nombre");
            }
            return Result<Flete>.Failure(MensajeError.atributoRequerido("nombre del fletero"));
        }

        internal async Task<Result<List<Flete>>> ObtenerTodosAsync()
        {
            List<Flete> fleteros = await this.fleteRepository.ObtenerTodosAsync();
            if (fleteros != null)
            {
                return Result<List<Flete>>.Success(fleteros);
            }
            return Result<List<Flete>>.Failure("Hubo un error al conectar con la base de datos");
        }

        internal async Task<Result<Flete>> ObtenerPorIdAsync(int id)
        {
            if (id < 0)
            {
                return Result<Flete>.Failure(MensajeError.idInvalido(id));
            }
            Flete fletero = await this.fleteRepository.ObtenerPorIdAsync(id);
            if (fletero != null)
            {
                return Result<Flete>.Success(fletero);
            }
            return Result<Flete>.Failure("No existe un fletero con ese id");
        }
    }
}
