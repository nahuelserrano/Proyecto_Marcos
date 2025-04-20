using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    class ViajeMixtoDTO
    {
        public string origen { get; set; }
        public string destino { get; set; }
        public DateOnly fecha_salida { get; set; }
        public string nombre_cliente { get; set; }
        public string nombre_chofer { get; set; }
        public string? camion { get; set; }
        public string? fletero { get; set; }
    }
}
