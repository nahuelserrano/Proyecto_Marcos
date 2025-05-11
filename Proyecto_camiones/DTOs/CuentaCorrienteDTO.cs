using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.DTOs
{
    public class CuentaCorrienteDTO
    {
        public int idCuenta { get; set; }
        public DateOnly Fecha_factura { get; set; }
        public int Nro_factura { get; set; }
        public float Adeuda { get; set; }
        public float Pagado { get; set; }

        public float Saldo_Total { get; set; }
        public int? idCliente { get; set; }
        public int? idFletero { get; set; }

        public CuentaCorrienteDTO(int id, DateOnly fecha_factura, int nro_factura, float adeuda, float pagado, float saldo_Total, int? idFletero, int? idCliente)
        {
            this.idCuenta = id;
            Fecha_factura = fecha_factura;
            Nro_factura = nro_factura;
            Adeuda = adeuda;
            Pagado = pagado;
            Saldo_Total = saldo_Total;
            this.idFletero = idFletero;
            this.idCliente = idCliente;
        }

        public CuentaCorrienteDTO()
        {

        }

        override
            public string ToString()
        {
            return "Cuenta nro: " + this.Nro_factura + ", fecha cuenta: " + this.Fecha_factura.ToString() + ", saldo total: " + this.Saldo_Total+", con id cuenta: "+this.idCuenta;
        }
    }
}
