using System;

namespace Proyecto_camiones.Models
{
    public class ViajeFlete
    {
        public int idViajeFlete { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public float remito { get; set; }
        public string carga { get; set; }
        public float km { get; set; }
        public float kg { get; set; }
        public float tarifa { get; set; }
        public int factura { get; set; }
        public int idCliente { get; set; }
        public int idFlete { get; set; }
        public string nombre_chofer { get; set; }
        public float comision { get; set; }
        public DateOnly fecha_salida { get; set; }

        public ViajeFlete(string? origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, int idCliente, int idFlete, string? nombre_chofer, float comision, DateOnly fecha)
        {
            this.origen = origen;
            this.destino = destino;
            this.remito = remito;
            this.carga = carga;
            this.km = km;
            this.kg = kg;
            this.tarifa = tarifa;
            this.factura = factura;
            this.idCliente = idCliente;
            this.idFlete = idFlete;
            this.nombre_chofer = nombre_chofer;
            this.comision = comision;
            this.fecha_salida = fecha;
        }

        public ViajeFlete()
        {
            this.origen = "default";
            this.carga = "default";
            this.nombre_chofer = "default";
        }


    }
}
