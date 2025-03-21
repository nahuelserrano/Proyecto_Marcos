using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cheque
    {
        public Cliente Cliente_Dueño_Cheque { get; set; }
        public DateTime FechaIngresoCheque { get; set; }
        public int NumeroCheque { get; set; }
        public float Monto { get; set; }
        public String Banco { get; set; }
        public DateTime FechaCobro { get; set; }

        public Cheque(Cliente cliente, DateTime FechaIngresoCheque, int NumeroCheque, float Monto, String Banco, DateTime FechaCobro)
        {
            this.Cliente_Dueño_Cheque = cliente;
            this.FechaIngresoCheque = DateTime.Now;
            this.NumeroCheque = NumeroCheque;
            this.Monto = Monto;
            this.Banco = Banco;
            this.FechaCobro = FechaCobro;
        }
    }
}