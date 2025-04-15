using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int Id_Chofer { get; set; }
        public DateOnly FechaDePago { get; set; }
        public float Monto_Pagado { get; set; }
        public DateOnly pagadoDesde { get; set; }
        public DateOnly pagadoHasta { get; set; }
        public float Monto { get; internal set; }
        public DateOnly FechaPago { get; internal set; }

        public Pago(float monto, int Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta, DateOnly FechaPago)
        {
            this.Id_Chofer = Id_Chofer;
            this.FechaDePago = FechaPago;
            this.Monto_Pagado = monto;
            this.pagadoDesde = pagadoDesde;
            this.pagadoHasta = pagadoHasta;
        }

    }
}