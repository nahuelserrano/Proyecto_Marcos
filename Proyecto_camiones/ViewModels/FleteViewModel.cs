using MySqlX.XDevAPI.Common;
using Proyecto_camiones.Models;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Repositories;
using Proyecto_camiones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.ViewModels
{
    public class FleteViewModel
    {

        public FleteService fleteService;

        public FleteViewModel()
        {
            var repo = new FleteRepository();
            fleteService = new FleteService(repo);
        }

        public async Task<bool> TestearConexion()
        {
            return await this.fleteService.TestearConexion();
        }

        public async Task<Result<int>> InsertarFletero(string nombre)
        {
            if (this.TestearConexion().Result)
            {
                return await this.fleteService.InsertarFletero(nombre);
            }
            return Result<int>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerFletePorNombre(string nombre)
        {
            if (this.TestearConexion().Result)
            {
                return await this.fleteService.ObtenerPorNombre(nombre);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexion con la base de datos");
        }

        public async Task<Result<List<Flete>>> ObtenerTodosAsync()
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ObtenerTodosAsync();
            }
            return Result<List<Flete>>.Failure("No se pudo establecer la conexión con la base de datos");
        }

        public async Task<Result<Flete>> ObtenerPorIdAsync(int id)
        {
            bool conexion = await this.TestearConexion();
            if (conexion)
            {
                return await this.fleteService.ObtenerPorIdAsync(id);
            }
            return Result<Flete>.Failure("No se pudo establecer la conexión con la base de datos");
        }
    }
}
