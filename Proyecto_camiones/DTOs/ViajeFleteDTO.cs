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

        public ViajeFleteDTO(string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, string cliente, string fletero, string nombre_chofer, float comision, DateOnly fecha_salida)
        {
            this.origen = origen;
            this.destino = destino;
            this.remito = remito;
            this.carga = carga;
            this.km = km;
            this.kg = kg;
            this.tarifa = tarifa;
            this.factura = factura;
            this.cliente = cliente;
            this.fletero = fletero;
            this.nombre_chofer = nombre_chofer;
            this.comision = comision;
            this.fecha_salida = fecha_salida;
        }

        public ViajeFleteDTO()
        {

        }

        override
            public string ToString()
        {
            return "ORIGEN: " + this.origen + ", DESTINO: " + this.destino + ", FLETERO: " + this.fletero + ", CLIENTE: " + this.cliente;
        }
    }
}
