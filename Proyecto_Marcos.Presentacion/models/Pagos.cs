using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Marcos.Presentacion.models
{
    class Pagos
    {
        public float monto { get; set; }
        public bool pagado { get; set; }

        public Pagos(float monto, bool pagado)
        {
            this.monto = monto;
            this.pagado = pagado;
        }
    }
}
