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
        public int? IdCliente { get; set; }
        public int? IdFletero { get; set; }
        public DateOnly Fecha_factura { get; set; }
        public int Nro_factura { get; set; }
        public float Adeuda { get; set; }
        public float Pagado { get; set; }

        public float Saldo_Total { get; set; }

        public CuentaCorriente(int? idCliente, int? idFletero, DateOnly fecha, int nro, float adeuda, float pagado, float ultimoSaldo)
        {
            this.IdCliente = idCliente;
            this.IdFletero = idFletero;
            this.Fecha_factura = fecha;
            this.Nro_factura = nro;
            this.Adeuda = adeuda;
            this.Pagado = pagado;
            this.Saldo_Total = this.Adeuda + ultimoSaldo - Pagado;
        }

        public CuentaCorriente()
        {
        }

        override
            public String ToString()
        {
            return "Cliente: " + this.IdCliente + " Adeuda: " + this.Adeuda + " Pagado: " + this.Pagado + " Saldo Total: " + this.Saldo_Total;
        }
    }
}
