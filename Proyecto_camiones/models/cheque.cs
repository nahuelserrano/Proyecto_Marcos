using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_camiones.Presentacion.Models
{
    public class Cheque
    {
        public int Id { get; set; }
        public int id_Cliente { get; set; }
        public DateTime FechaIngresoCheque { get; set; }
        public string NumeroCheque { get; set; }
        public float Monto { get; set; }
        public String Banco { get; set; }
        public DateTime FechaCobro { get; set; }

        public Cheque(int id_Cliente, DateTime FechaIngresoCheque, string NumeroCheque, float Monto, String Banco, DateTime FechaCobro)
        {
            this.id_Cliente = id_Cliente;
            this.FechaIngresoCheque = FechaIngresoCheque;
            this.NumeroCheque = NumeroCheque;
            this.Monto = Monto;
            this.Banco = Banco;
            this.FechaCobro = FechaCobro;
        }

        public Cheque()
        {
            this.id_Cliente = 1;
            this.Banco = "bna";
        }
    }
}