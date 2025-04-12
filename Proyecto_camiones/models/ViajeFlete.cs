using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class ViajeFlete
    {
        private int idViajeFlete { get; set; }
        private string origen { get; set; }
        private string destino { get; set; }
        private float remito { get; set; }
        private string carga { get; set; }
        private float km { get; set; }
        private float kg { get; set; }
        private float tarifa { get; set; }
        private int factura { get; set; }
        private int idCliente { get; set; }
        private int idFlete { get; set; }

        public ViajeFlete(int idViajeFlete, string origen, string destino, float remito, string carga, float km, float kg, float tarifa, int factura, int idCliente, int idFlete)
        {
            this.idViajeFlete = idViajeFlete;
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
        }

        
    }
}
