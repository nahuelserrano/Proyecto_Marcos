using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class ViajeMixtoDTO
    {
        public string Origen { get; set; }
        public string Destino { get; set; }
        public DateOnly Fecha_salida { get; set; }
        public float Remito { get; set; }
        public string Nombre_chofer { get; set; }
        public string Carga { get; set; }
        public float Km { get; set; }
        public float Kg { get; set; }
        public float Tarifa { get; set; }
        public float Total => Kg * Tarifa;
        public float? Total_comision { get; set; }
        public float? Comision { get; set; }
        public string? Camion { get; set; }
        public string? Fletero { get; set; }

        //CONSTRUCTOR PARA EL VIAJE PERSONAL
        public ViajeMixtoDTO(string origen, string destino, DateOnly fecha_salida, float remito, string nombre_chofer, string carga, float km, float kg, float tarifa, string camion)
        {
            this.Origen = origen;
            this.Destino = destino;
            this.Fecha_salida = fecha_salida;
            this.Remito = remito;
            this.Nombre_chofer = nombre_chofer;
            this.Carga = carga;
            this.Km = km;
            this.Kg = kg;
            this.Tarifa = tarifa;
            this.Comision = null;
            this.Total_comision = null;
            this.Camion = camion;
            this.Fletero = null;
        }

        //CONSTRUCTOR PARA VIAJES POR FLETERO

        public ViajeMixtoDTO(string origen, string destino, DateOnly fecha_salida, float remito, string nombre_chofer, string carga, float km, float kg, float tarifa, float comision, string fletero)
        {
            this.Origen = origen;
            this.Destino = destino;
            this.Fecha_salida = fecha_salida;
            this.Remito = remito;
            this.Nombre_chofer = nombre_chofer;
            this.Carga = carga;
            this.Km = km;
            this.Kg = kg;
            this.Tarifa = tarifa;
            this.Comision = comision;
            this.Total_comision = this.Comision * this.Total / 100;
            this.Fletero = fletero;
            this.Camion = null;
        }

        public ViajeMixtoDTO() { }

        override
            public String ToString()
        {
            return "Origen: " + this.Origen + ", DESTINO: " + this.Destino + " , TOTAL: " + this.Total + ", TOTAL COMISION: " + this.Total_comision + " , FLETERO:" + this.Fletero;
        }
    }
}
