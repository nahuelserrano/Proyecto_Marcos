using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeMixtoDTO
    {
        public string origen { get; set; }
        public string destino { get; set; }
        public DateOnly fecha_salida { get; set; }
        public float remito { get; set; }
        public string nombre_chofer { get; set; }
        public string carga { get; set; }
        public float km { get; set; }
        public float kg { get; set; }
        public float tarifa { get; set; }
        public float total { get; set; }
        public float? total_comision { get; set; }
        public float? comision { get; set; }
        public string? camion { get; set; }
        public string? fletero { get; set; }

        //CONSTRUCTOR PARA EL VIAJE PERSONAL
        public ViajeMixtoDTO(string origen, string destino, DateOnly fecha_salida, float remito, string nombre_chofer, string carga, float km, float kg, float tarifa, string camion)
        {
            this.origen = origen;
            this.destino = destino;
            this.fecha_salida = fecha_salida;
            this.remito = remito;
            this.nombre_chofer = nombre_chofer;
            this.carga = carga;
            this.km = km;
            this.kg = kg;
            this.tarifa = tarifa;
            this.total = this.kg * this.tarifa;
            this.comision = null;
            this.total_comision = null;
            this.camion = camion;
            this.fletero = null;
        }

        //CONSTRUCTOR PARA VIAJES POR FLETERO

        public ViajeMixtoDTO(string origen, string destino, DateOnly fecha_salida, float remito, string nombre_chofer, string carga, float km, float kg, float tarifa, float comision, string fletero)
        {
            this.origen = origen;
            this.destino = destino;
            this.fecha_salida = fecha_salida;
            this.remito = remito;
            this.nombre_chofer = nombre_chofer;
            this.carga = carga;
            this.km = km;
            this.kg = kg;
            this.tarifa = tarifa;
            this.total = this.kg * this.tarifa;
            this.comision = comision;
            this.total_comision = this.comision * this.total / 100;
            this.fletero = fletero;
            this.camion = null;
        }

        override
            public String ToString()
        {
            return "Origen: " + this.origen + ", DESTINO: " + this.destino + " , TOTAL: " + this.total + ", TOTAL COMISION: " + this.total_comision + " , FLETERO:" + this.fletero;
        }
    }
}
