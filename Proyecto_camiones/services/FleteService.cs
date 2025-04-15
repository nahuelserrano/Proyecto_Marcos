using MySqlX.XDevAPI.Common;
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
                int id = await this.fleteRepository.InsertarFletero(nombre);
                if (id > -1)
                {
                    return Result<int>.Success(id);
                }
                return Result<int>.Failure("Hubo un problema al intentar insertar el fletero");
            }
            return Result<int>.Failure("El campo nombre no puede ser nulo");
        }
    }
}
