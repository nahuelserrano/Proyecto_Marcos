using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeFleteDTO
    {
        public string origen { get; set; }
        public string destino { get; set; }
        public float remito { get; set; }
        public string carga { get; set; }
        public float km { get; set; }
        public float kg { get; set; }
        public float tarifa { get; set; }
        public int factura { get; set; }
        public string cliente { get; set; }
        public string fletero { get; set; }
        public string nombre_chofer { get; set; }
        public float comision { get; set; }
        public DateOnly fecha_salida { get; set; }
    }
}
