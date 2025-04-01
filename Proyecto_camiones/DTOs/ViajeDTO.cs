using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeDTO
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string LugarPartida { get; set; }
        public string Destino { get; set; }
        public int Carga { get; set; }
        public int Km { get; set; }
        public double PrecioPorKilo { get; set; }
        public string Remito { get; set; }
        public string nombreChofer { get; set; }
        public string nombreCliente { get; set; }

        public ViajeDTO()
        {

        }
    }
}
