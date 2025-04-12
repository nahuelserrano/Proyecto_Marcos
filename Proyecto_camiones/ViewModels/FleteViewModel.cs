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
        private FleteService fleteService;

        public FleteViewModel()
        {
            var dbContext = General.obtenerInstancia();
            var repo = new FleteRepository(dbContext);
            this.fleteService = new FleteService(repo);
        }


        public async Task<bool> testearConexion()
        {
            return await this.fleteService.ProbarConexionAsync();
        }
    }
}
