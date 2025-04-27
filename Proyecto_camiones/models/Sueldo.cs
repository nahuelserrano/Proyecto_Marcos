using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Sueldo
    {
        public int Id { get; set; }
        public int Id_Chofer { get; set; }
        public DateOnly pagadoDesde { get; set; }
        public DateOnly pagadoHasta { get; set; }
        public float Monto { get; internal set; }
        public DateOnly FechaPago { get; internal set; }

        public Sueldo(float monto, int Id_Chofer, DateOnly pagadoDesde, DateOnly pagadoHasta)
        {
            this.Id_Chofer = Id_Chofer;
            this.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            this.Monto = monto;
            this.pagadoDesde = pagadoDesde;
            this.pagadoHasta = pagadoHasta;
        }
    }
}