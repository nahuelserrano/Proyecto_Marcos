using Proyecto_camiones.Core.Services;
using Proyecto_camiones.DTOs;
using Proyecto_camiones.Presentacion.Repositories;
using Proyecto_camiones.Presentacion.Services;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class SueldoViewModel
    {

        private readonly ISueldoService _sueldoService;

        public SueldoViewModel(ISueldoService sueldoService)
        {
            _sueldoService = sueldoService ?? throw new ArgumentNullException(nameof(sueldoService));
        }

        public async Task<bool> testearConexion()
        {
            return await this._sueldoService.ProbarConexionAsync();
        }

        public async Task<Result<int>> CrearAsync(string nombre_chofer, DateOnly pagoDesde, DateOnly pagoHasta, DateOnly? fechaPago, string? patente_camion)
        {
            if (await this.testearConexion())
            {
                Console.WriteLine("hola view model hay conexion");
                return await this.sueldoService.CrearAsync(nombre_chofer, pagoDesde, pagoHasta,fechaPago, patente_camion);

            }
            return Result<int>.Failure("La conexión no pude establecerse");
        }

        public async Task<Result<SueldoDTO>> marcarPago(int idSueldo)
        {
            if (await this.testearConexion())
            {
                return await this.sueldoService.marcarPagado(idSueldo);
            }
            return Result<SueldoDTO>.Failure("No se pudo establecer la conexión");
        }
        public async Task<Result<List<SueldoDTO>>> ObtenerTodosAsync(string? patente,string? nombreChofer) {
            if (await this.testearConexion())
            {
                Result<List<SueldoDTO>> sueldos = await this.sueldoService.ObtenerTodosAsync(patente, nombreChofer);

                if (sueldos!=null)
                return sueldos;
            }
            return Result<List<SueldoDTO>>.Failure("No se pudo establecer la conexión");
        }

        public async Task<Result<bool>> EliminarAsync(int id)   
        {
            if (await this.testearConexion())
            {
                return await this.sueldoService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión");
        }
    }
}
