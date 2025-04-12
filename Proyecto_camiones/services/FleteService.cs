using Proyecto_camiones.Presentacion.Models;
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
        private FleteRepository fleteRepository;

        public FleteService(FleteRepository fleteRepository)
        {
            this.fleteRepository = fleteRepository ?? throw new ArgumentNullException(nameof(fleteRepository));
        }

        public async Task<bool> ProbarConexionAsync()
        {
            bool result = await this.fleteRepository.ProbarConexionAsync();
            return result;
        }
    }
}
