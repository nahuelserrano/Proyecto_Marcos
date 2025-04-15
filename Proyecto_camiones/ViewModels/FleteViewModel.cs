/*
using MySqlX.XDevAPI.Common;
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
            var dbContext = General.obtenerInstancia();
            var repo = new FleteRepository(dbContext);
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
    }
}
*/