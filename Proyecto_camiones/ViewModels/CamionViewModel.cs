using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.ViewModels
{
    public class CamionViewModel
    {
        private CamionService _camionService;

        public CamionViewModel()
        {
            var camionRepository = new CamionRepository();
            this._camionService = new CamionService(camionRepository);
        }

        public async Task<bool> testearConexion()
        {
            return await this._camionService.ProbarConexionAsync();
        }

        public async Task<Result<int>> InsertarAsync( string patente, string nombre)
        {
            bool result = await this.testearConexion();
            if (result)
            {
                var resultado = await this._camionService.CrearAsync(patente, nombre);

                // Ahora puedes acceder al resultado
                if (resultado.IsSuccess)
                {
                    // La operación fue exitosa
                    int idCamion = resultado.Value;
                    return resultado;
                }
                else
                {
                    // Si la operación falló, maneja el error
                    return Result<int>.Failure(resultado.Error);
                }
            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }

        public async Task<Result<List<CamionDTO>>> ObtenerTodosAsync() 
        {
            bool result = await this.testearConexion();
            if (result)
            {
                var camiones = await this._camionService.ObtenerTodosAsync();
                return Result<List<CamionDTO>>.Success(camiones);
            }
            return Result<List<CamionDTO>>.Failure("La conexión no pudo establecerse");
        }

        public async Task<Result<CamionDTO>> ActualizarAsync(int id, string? patente, string? nombre)
        {
            if (this.testearConexion().Result)
            {
                Result<CamionDTO> camion = await this._camionService.ActualizarAsync(id, patente, nombre);
                if(camion.IsSuccess)
                {
                    return camion;
                }
                return Result<CamionDTO>.Failure(camion.Error);
            }
            return Result<CamionDTO>.Failure("No pudo establecerse la conexión");
        }

        public async Task<Result<string>> EliminarAsync(int id)
        {
            if (await this.testearConexion())
            {
                return await this._camionService.EliminarAsync(id);
            }
            return Result<string>.Failure("error de conexión");
        }

        public async Task<Result<CamionDTO>> ObtenerPorId(int id)
        {
            bool result = await this.testearConexion();
            if (result)
            {
                CamionDTO camion = await this._camionService.ObtenerPorIdAsync(id);
                if(camion != null)
                {
                    return Result<CamionDTO>.Success(camion);
                }
                return Result<CamionDTO>.Failure("No se pudo obtener el camión con ese id");
            }
            return Result<CamionDTO>.Failure("error de conexión");
        }


    }
}
