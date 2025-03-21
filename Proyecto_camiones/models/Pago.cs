using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public float Monto { get; set; }
        public bool Pagado { get; set; }

        public Pago(int id, float monto, bool pagado)
        {
            this.Id = id;
            this.Monto = monto;
            this.Pagado = pagado;
        }
    }
}