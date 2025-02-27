using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransporteApp.Models;

namespace TransporteApp.Repositories
{
    public class ViajeRepository
    {
        public Object db { get; set; }

        internal async Task<int> InsertarViajeAsync(Viaje viaje)
        {
            throw new NotImplementedException();
        }
    }
}
