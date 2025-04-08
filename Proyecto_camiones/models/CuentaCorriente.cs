using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Models
{
    public class CuentaCorriente
    {
        public int Id { get; set;}
        public int IdCliente { get; set; }
        public DateOnly Fecha_factura { get; set; }
        public int Nro_factura { get; set; }
        public float Adeuda { get; set; }
        public float Pagado { get; set; }

        public CuentaCorriente(int idCliente, DateOnly fecha, int nro, float adeuda, float pagado)
        {
            this.IdCliente = idCliente;
            this.Fecha_factura = fecha;
            this.Nro_factura = nro;
            this.Adeuda = adeuda;
            this.Pagado = pagado;
        }

        public CuentaCorriente()
        {
        }
    }
}
