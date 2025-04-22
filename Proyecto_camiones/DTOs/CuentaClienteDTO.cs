using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class CuentaClienteDTO
    {
        public DateOnly Fecha_factura { get; set; }
        public int Nro_factura { get; set; }
        public float Adeuda { get; set; }
        public float Pagado { get; set; }

        public float Saldo_Total { get; set; }

        public CuentaClienteDTO(DateOnly fecha_factura, int nro_factura, float adeuda, float pagado, float saldo_Total)
        {
            Fecha_factura = fecha_factura;
            Nro_factura = nro_factura;
            Adeuda = adeuda;
            Pagado = pagado;
            Saldo_Total = saldo_Total;
        }

        public CuentaClienteDTO()
        {

        }

        override
            public string ToString()
        {
            return "Cuenta nro: " + this.Nro_factura + ", fecha cuenta: " + this.Fecha_factura.ToString() + ", saldo total: " + this.Saldo_Total;
        }
    }
}
