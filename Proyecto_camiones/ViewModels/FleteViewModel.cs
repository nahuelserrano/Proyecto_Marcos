using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_camiones.Core.Services;
using System;

namespace Proyecto_camiones.ViewModels
{
    public class FleteViewModel
    {

        private readonly IFleteService _fleteService;

        public FleteViewModel(IFleteService fleteService)
        {
            _fleteService = fleteService ?? throw new ArgumentNullException(nameof(fleteService));
        }

        public async Task<bool> TestearConexion()
        {
            return await this._fleteService.TestearConexion();
        }

        public async Task<Result<int>> InsertarAsync(string nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.InsertarFletero(nombre);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerFletePorNombreAsync(string nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.ObtenerPorNombreAsync(nombre);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<List<Flete>>> ObtenerTodosAsync()
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.ObtenerTodosAsync();
            }
            return Result<List<Flete>>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerPorIdAsync(int id)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.ObtenerPorIdAsync(id);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<bool>> EliminarAsync(int id)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.EliminarAsync(id);
            }
            return Result<bool>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<Flete>> ActualizarAsync(int id, string? nombre)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this._fleteService.ActualizarAsync(id, nombre);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexión con la base de datos");
        }
    }
}
